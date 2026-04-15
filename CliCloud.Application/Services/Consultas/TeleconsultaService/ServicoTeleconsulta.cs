using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs;
using CliCloud.Application.Services.Consultas.TeleconsultaService.Specifications;
using CliCloud.Application.Services.Medicos.MedicoService;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService
{
  public class ServicoTeleconsulta(
    IRepositoryAsync repository,
    ICurrentTenantUserService currentTenantUserService,
    IMedicoService medicoService,
    IHttpContextAccessor httpContextAccessor
  ) : IServicoTeleconsulta
  {
    private readonly IRepositoryAsync _repository = repository;
    private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;
    private readonly IMedicoService _medicoService = medicoService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string DefaultBaseMeetingUrl = "https://meet.jit.si";

    public async Task<Response<TeleconsultaSessaoDTO>> CriarOuObterSessaoAsync(Guid clinicaId, CriarTeleconsultaRequest request)
    {
      try
      {
        var consultaMarcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(request.ConsultaMarcacaoId);
        if (consultaMarcacao == null)
          return ResponseFactory.Fail<TeleconsultaSessaoDTO>("Marcação de consulta não encontrada.");

        var config = await ObterConfiguracaoTeleconsultaAsync(clinicaId);
        if (config == null || !config.Ativo)
          return ResponseFactory.Fail<TeleconsultaSessaoDTO>("A teleconsulta não está ativa para esta clínica.");

        var validacaoProfissional = await ValidarProfissionalDaMarcacaoAsync(consultaMarcacao);
        if (validacaoProfissional is not null)
          return ResponseFactory.Fail<TeleconsultaSessaoDTO>(validacaoProfissional);

        var specExistente = new TeleconsultaPorClinicaMarcacaoSpec(clinicaId, request.ConsultaMarcacaoId);
        var existente = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(specExistente))
          .OrderByDescending(x => x.CreatedOn)
          .FirstOrDefault(x => x.Ativo);

        if (existente != null)
          return ResponseFactory.Success(MapToDto(existente));

        var inicio = ResolverInicioPrevistoUtc(consultaMarcacao, request.InicioPrevistoUtc);
        var duracaoPadrao = config.DuracaoPadraoMinutos is >= 5 and <= 360 ? config.DuracaoPadraoMinutos : 30;
        var duracao = request.DuracaoMinutos is >= 5 and <= 360 ? request.DuracaoMinutos.Value : duracaoPadrao;
        var fim = inicio.AddMinutes(duracao);

        var meetingId = $"clicloud-{GerarTokenSeguro(24)}";
        var meetingUrl = $"{NormalizarBaseMeetingUrl(config.BaseMeetingUrl)}/{meetingId}";

        var entidade = new TeleconsultaSessao
        {
          ClinicaId = clinicaId,
          ConsultaMarcacaoId = request.ConsultaMarcacaoId,
          MeetingId = meetingId,
          MeetingUrl = meetingUrl,
          Provider = string.IsNullOrWhiteSpace(config.Provider) ? "jitsi" : config.Provider.Trim(),
          Status = "Criada",
          InicioPrevistoUtc = inicio,
          FimPrevistoUtc = fim,
          TokenMedico = GerarTokenSeguro(48),
          TokenUtente = GerarTokenSeguro(48),
          LinksAtivos = true,
          Ativo = true,
        };

        var criada = await _repository.CreateAsync<TeleconsultaSessao, Guid>(entidade);
        await _repository.SaveChangesAsync();
        await RegistarAuditoriaAsync(criada, "CriarSessao", "Profissional", true, "Sessão de teleconsulta criada.");

        return ResponseFactory.Success(MapToDto(criada));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TeleconsultaSessaoDTO>(ex.Message);
      }
    }

    public async Task<Response<TeleconsultaSessaoDTO>> ObterPorMarcacaoAsync(Guid clinicaId, Guid consultaMarcacaoId)
    {
      try
      {
        var spec = new TeleconsultaPorClinicaMarcacaoSpec(clinicaId, consultaMarcacaoId);
        var entidade = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(spec))
          .OrderByDescending(x => x.CreatedOn)
          .FirstOrDefault();

        if (entidade == null)
          return ResponseFactory.Fail<TeleconsultaSessaoDTO>("Sessão de teleconsulta não encontrada.");

        return ResponseFactory.Success(MapToDto(entidade));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TeleconsultaSessaoDTO>(ex.Message);
      }
    }

    public async Task<Response<TeleconsultaJoinDTO>> ObterLinkEntradaAsync(Guid clinicaId, Guid sessaoId, EntrarTeleconsultaRequest request)
    {
      try
      {
        var config = await ObterConfiguracaoTeleconsultaAsync(clinicaId);
        if (config == null || !config.Ativo)
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("A teleconsulta não está ativa para esta clínica.");

        var spec = new TeleconsultaPorClinicaSessaoSpec(clinicaId, sessaoId);
        var entidade = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(spec)).FirstOrDefault();
        if (entidade == null)
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("Sessão de teleconsulta não encontrada.");

        var consultaMarcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(entidade.ConsultaMarcacaoId);
        if (consultaMarcacao == null)
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("Marcação associada à sessão de teleconsulta não encontrada.");

        if (!entidade.Ativo || string.Equals(entidade.Status, "Cancelada", StringComparison.OrdinalIgnoreCase))
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("Sessão inativa ou cancelada.");

        if (!entidade.LinksAtivos)
        {
          await RegistarAuditoriaAsync(entidade, "Entrar", request.Papel, false, "Tentativa com links revogados.");
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("Os links desta sessão foram revogados.");
        }

        var agora = DateTime.UtcNow;
        var janelaMinutos = config.JanelaEntradaMinutosAntes < 0 ? 0 : config.JanelaEntradaMinutosAntes;
        if (!config.PermitirEntradaAntesDoInicio)
        {
          var inicioPermitido = entidade.InicioPrevistoUtc.AddMinutes(-janelaMinutos);
          if (agora < inicioPermitido)
          {
            await RegistarAuditoriaAsync(entidade, "Entrar", request.Papel, false, "Tentativa fora da janela de entrada.");
            return ResponseFactory.Fail<TeleconsultaJoinDTO>("A entrada ainda não está disponível para esta teleconsulta.");
          }
        }

        if (agora > entidade.FimPrevistoUtc.AddMinutes(Math.Max(15, janelaMinutos)))
        {
          await RegistarAuditoriaAsync(entidade, "Entrar", request.Papel, false, "Tentativa com sessão expirada.");
          return ResponseFactory.Fail<TeleconsultaJoinDTO>("A janela de entrada da teleconsulta expirou.");
        }

        var papelNormalizado = NormalizarPapel(request.Papel);
        var eModerador = string.Equals(papelNormalizado, "Profissional", StringComparison.OrdinalIgnoreCase);

        if (eModerador)
        {
          var validacaoProfissional = await ValidarProfissionalDaMarcacaoAsync(consultaMarcacao);
          if (validacaoProfissional is not null)
            return ResponseFactory.Fail<TeleconsultaJoinDTO>(validacaoProfissional);
        }
        else
        {
          if (!string.IsNullOrWhiteSpace(request.CodigoAcesso))
          {
            if (!string.Equals(request.CodigoAcesso.Trim(), entidade.TokenUtente, StringComparison.Ordinal))
            {
              await RegistarAuditoriaAsync(entidade, "Entrar", "Utente", false, "Código de acesso inválido.");
              return ResponseFactory.Fail<TeleconsultaJoinDTO>("Código de acesso de utente inválido.");
            }
          }
        }

        if (string.Equals(entidade.Status, "Criada", StringComparison.OrdinalIgnoreCase))
        {
          entidade.Status = "Aberta";
          entidade.InicioEfetivoUtc ??= agora;
          await _repository.UpdateAsync<TeleconsultaSessao, Guid>(entidade);
          await _repository.SaveChangesAsync();
        }

        var url = entidade.MeetingUrl;
        var expiracao = entidade.FimPrevistoUtc.AddMinutes(Math.Max(15, janelaMinutos));

        if (config.JwtAtivo)
        {
          if (string.IsNullOrWhiteSpace(config.JwtPrivateKey))
            return ResponseFactory.Fail<TeleconsultaJoinDTO>("JWT ativo sem private key configurada.");

          var token = GerarJwtJitsi(config, entidade, request.NomeExibicao, eModerador, expiracao);
          var separador = url.Contains('?') ? "&" : "?";
          url = $"{url}{separador}jwt={Uri.EscapeDataString(token)}";
        }

        if (!string.IsNullOrWhiteSpace(request.NomeExibicao))
          url = $"{url}#userInfo.displayName=\"{Uri.EscapeDataString(request.NomeExibicao.Trim())}\"";

        var dto = new TeleconsultaJoinDTO
        {
          SessaoId = entidade.Id,
          MeetingId = entidade.MeetingId,
          MeetingUrl = url,
          Papel = papelNormalizado,
          Moderador = eModerador,
          ExpiraEmUtc = expiracao,
        };

        await RegistarAuditoriaAsync(entidade, "Entrar", papelNormalizado, true, "Link de entrada emitido.");

        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TeleconsultaJoinDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> TerminarSessaoAsync(Guid clinicaId, Guid sessaoId)
    {
      try
      {
        var spec = new TeleconsultaPorClinicaSessaoSpec(clinicaId, sessaoId);
        var entidade = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(spec)).FirstOrDefault();
        if (entidade == null)
          return ResponseFactory.Fail<Guid>("Sessão de teleconsulta não encontrada.");

        entidade.Status = "Terminada";
        entidade.FimEfetivoUtc = DateTime.UtcNow;
        entidade.LinksAtivos = false;
        entidade.LinksRevogadosEmUtc = DateTime.UtcNow;
        entidade.TokenMedico = GerarTokenSeguro(48);
        entidade.TokenUtente = GerarTokenSeguro(48);
        entidade.Ativo = false;

        await _repository.UpdateAsync<TeleconsultaSessao, Guid>(entidade);
        await _repository.SaveChangesAsync();
        await RegistarAuditoriaAsync(entidade, "TerminarSessao", "Profissional", true, "Sessão terminada e links revogados.");
        return ResponseFactory.Success(entidade.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<TeleconsultaJoinDTO>> GerarLinkUtenteAsync(
      Guid clinicaId,
      Guid sessaoId,
      GerarLinkUtenteRequest request
    )
    {
      var nome = string.IsNullOrWhiteSpace(request.NomeExibicao) ? "Utente" : request.NomeExibicao.Trim();
      var resultado = await ObterLinkEntradaAsync(
        clinicaId,
        sessaoId,
        new EntrarTeleconsultaRequest
        {
          Papel = "Utente",
          NomeExibicao = nome,
          CodigoAcesso = null,
        }
      );

      if (resultado.Status == ResponseStatus.Success && resultado.Data != null)
      {
        var spec = new TeleconsultaPorClinicaSessaoSpec(clinicaId, sessaoId);
        var entidade = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(spec)).FirstOrDefault();
        if (entidade != null)
        {
          var destino = string.IsNullOrWhiteSpace(request.Destino) ? "N/D" : request.Destino.Trim();
          await RegistarAuditoriaAsync(
            entidade,
            "GerarLinkUtente",
            "Profissional",
            true,
            $"Link de utente gerado para destino: {destino}."
          );
        }
      }

      return resultado;
    }

    public async Task<Response<Guid>> RevogarLinksSessaoAsync(Guid clinicaId, Guid sessaoId, string motivo = "Revogação manual")
    {
      try
      {
        var spec = new TeleconsultaPorClinicaSessaoSpec(clinicaId, sessaoId);
        var entidade = (await _repository.GetListAsync<TeleconsultaSessao, Guid>(spec)).FirstOrDefault();
        if (entidade == null)
          return ResponseFactory.Fail<Guid>("Sessão de teleconsulta não encontrada.");

        entidade.LinksAtivos = false;
        entidade.LinksRevogadosEmUtc = DateTime.UtcNow;
        entidade.TokenMedico = GerarTokenSeguro(48);
        entidade.TokenUtente = GerarTokenSeguro(48);

        if (!string.Equals(entidade.Status, "Terminada", StringComparison.OrdinalIgnoreCase))
          entidade.Status = "LinksRevogados";

        await _repository.UpdateAsync<TeleconsultaSessao, Guid>(entidade);
        await _repository.SaveChangesAsync();
        await RegistarAuditoriaAsync(entidade, "RevogarLinks", "Profissional", true, motivo);
        return ResponseFactory.Success(entidade.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static DateTime ResolverInicioPrevistoUtc(ConsultaMarcacao consultaMarcacao, DateTime? inicioRequestUtc)
    {
      if (inicioRequestUtc.HasValue)
        return DateTime.SpecifyKind(inicioRequestUtc.Value, DateTimeKind.Utc);

      if (consultaMarcacao.Data.HasValue)
      {
        var data = consultaMarcacao.Data.Value.Date;
        var hora = consultaMarcacao.HoraMarcacao ?? TimeSpan.Zero;
        var local = data.Add(hora);
        return DateTime.SpecifyKind(local, DateTimeKind.Local).ToUniversalTime();
      }

      return DateTime.UtcNow;
    }

    private async Task<ConfiguracaoTeleconsulta?> ObterConfiguracaoTeleconsultaAsync(Guid clinicaId)
    {
      var configs = await _repository.GetListAsync<ConfiguracaoTeleconsulta, Guid>();
      return configs.FirstOrDefault(x => x.ClinicaId == clinicaId);
    }

    private async Task<string?> ValidarProfissionalDaMarcacaoAsync(ConsultaMarcacao marcacao)
    {
      _currentTenantUserService.SetUser();
      var userIdStr = _currentTenantUserService.UserId;

      if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        return "Utilizador autenticado inválido para operação de teleconsulta.";

      var medicoAtualResult = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
      if (medicoAtualResult.Status != ResponseStatus.Success || medicoAtualResult.Data == null)
        return "Apenas profissionais clínicos podem criar ou entrar como moderador na teleconsulta.";

      var medicoAtual = medicoAtualResult.Data;

      if (marcacao.MedicoId.HasValue && marcacao.MedicoId.Value != medicoAtual.Id)
        return "O profissional autenticado não está associado à marcação desta teleconsulta.";

      if (!marcacao.MedicoId.HasValue)
      {
        marcacao.MedicoId = medicoAtual.Id;
        await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
        await _repository.SaveChangesAsync();
      }

      return null;
    }

    private static string NormalizarBaseMeetingUrl(string? configuredBaseUrl)
    {
      var baseUrl = string.IsNullOrWhiteSpace(configuredBaseUrl) ? DefaultBaseMeetingUrl : configuredBaseUrl.Trim();
      return baseUrl.TrimEnd('/');
    }

    private static string NormalizarPapel(string? papel)
    {
      if (string.IsNullOrWhiteSpace(papel))
        return "Profissional";

      var p = papel.Trim();
      if (p.Equals("medico", StringComparison.OrdinalIgnoreCase) || p.Equals("moderador", StringComparison.OrdinalIgnoreCase))
        return "Profissional";
      if (p.Equals("utente", StringComparison.OrdinalIgnoreCase) || p.Equals("paciente", StringComparison.OrdinalIgnoreCase))
        return "Utente";

      return "Profissional";
    }

    private static string GerarTokenSeguro(int tamanho)
    {
      var bytes = new byte[tamanho];
      RandomNumberGenerator.Fill(bytes);
      return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private async Task RegistarAuditoriaAsync(
      TeleconsultaSessao sessao,
      string acao,
      string? papel,
      bool sucesso,
      string? mensagem
    )
    {
      _currentTenantUserService.SetUser();
      var userId = _currentTenantUserService.UserId;
      var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
      var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].FirstOrDefault();

      var log = new TeleconsultaAcessoLog
      {
        ClinicaId = sessao.ClinicaId,
        TeleconsultaSessaoId = sessao.Id,
        ConsultaMarcacaoId = sessao.ConsultaMarcacaoId,
        UserId = userId,
        Papel = string.IsNullOrWhiteSpace(papel) ? "Sistema" : papel.Trim(),
        Acao = acao,
        Mensagem = mensagem,
        Sucesso = sucesso,
        Ip = string.IsNullOrWhiteSpace(ip) ? null : ip,
        UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
      };

      await _repository.CreateAsync<TeleconsultaAcessoLog, Guid>(log);
      await _repository.SaveChangesAsync();
    }

    private static string GerarJwtJitsi(
      ConfiguracaoTeleconsulta config,
      TeleconsultaSessao entidade,
      string? nomeExibicao,
      bool moderador,
      DateTime expiraEmUtc
    )
    {
      var baseUrl = NormalizarBaseMeetingUrl(config.BaseMeetingUrl);
      var host = new Uri(baseUrl).Host;
      var privateKey = config.JwtPrivateKey?.Trim() ?? string.Empty;
      var issuer = string.IsNullOrWhiteSpace(config.JwtAppId) ? host : config.JwtAppId!.Trim();
      var audience = string.IsNullOrWhiteSpace(config.JwtApiKey) ? "jitsi" : config.JwtApiKey!.Trim();
      var nome = string.IsNullOrWhiteSpace(nomeExibicao)
        ? (moderador ? "Profissional de Saúde" : "Utente")
        : nomeExibicao.Trim();
      SigningCredentials creds;
      SecurityKey securityKey;
      if (PareceChavePemPrivada(privateKey))
      {
        try
        {
          using var rsa = RSA.Create();
          rsa.ImportFromPem(privateKey);
          var rsaKey = new RsaSecurityKey(rsa.ExportParameters(true));
          if (!string.IsNullOrWhiteSpace(config.JwtKid))
            rsaKey.KeyId = config.JwtKid!.Trim();

          securityKey = rsaKey;
          creds = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }
        catch (Exception ex)
        {
          throw new InvalidOperationException("JWT Private Key inválida para assinatura RSA (JaaS).", ex);
        }
      }
      else
      {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        if (!string.IsNullOrWhiteSpace(config.JwtKid))
          symmetricKey.KeyId = config.JwtKid!.Trim();

        securityKey = symmetricKey;
        creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      }

      var claims = new List<Claim>
      {
        new(
          "context",
          $"{{\"user\":{{\"name\":\"{nome}\",\"moderator\":{moderador.ToString().ToLowerInvariant()}}}}}"
        ),
      };

      var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        notBefore: DateTime.UtcNow.AddMinutes(-1),
        expires: expiraEmUtc,
        signingCredentials: creds
      );

      if (!string.IsNullOrWhiteSpace(config.JwtKid))
        token.Header["kid"] = config.JwtKid!.Trim();

      token.Payload["sub"] = host;
      token.Payload["room"] = entidade.MeetingId;
      token.Payload["moderator"] = moderador;

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool PareceChavePemPrivada(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return false;

      return value.Contains("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal)
        || value.Contains("-----BEGIN RSA PRIVATE KEY-----", StringComparison.Ordinal);
    }

    private static TeleconsultaSessaoDTO MapToDto(TeleconsultaSessao entidade)
    {
      return new TeleconsultaSessaoDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        ConsultaMarcacaoId = entidade.ConsultaMarcacaoId,
        MeetingId = entidade.MeetingId,
        MeetingUrl = entidade.MeetingUrl,
        Provider = entidade.Provider,
        Status = entidade.Status,
        InicioPrevistoUtc = entidade.InicioPrevistoUtc,
        FimPrevistoUtc = entidade.FimPrevistoUtc,
        InicioEfetivoUtc = entidade.InicioEfetivoUtc,
        FimEfetivoUtc = entidade.FimEfetivoUtc,
        LinksAtivos = entidade.LinksAtivos,
        LinksRevogadosEmUtc = entidade.LinksRevogadosEmUtc,
        Ativo = entidade.Ativo,
      };
    }
  }
}
