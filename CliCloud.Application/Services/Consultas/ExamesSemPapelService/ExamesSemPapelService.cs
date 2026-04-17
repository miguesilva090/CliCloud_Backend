using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ExamesSemPapelService.DTOs;
using CliCloud.Application.Services.Consultas.ExamesSemPapelService.Specifications;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Exames;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utentes;
using System.Globalization;

namespace CliCloud.Application.Services.Consultas.ExamesSemPapelService;

public class ExamesSemPapelService(
  IRepositoryAsync repository,
  ICurrentTenantUserService currentTenantUserService
) : IExamesSemPapelService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;

  private static readonly List<AreaPrestacaoOpcaoDTO> AreasPrestacao = [
    new() { Codigo = "C", Descricao = "Consultas" },
    new() { Codigo = "F", Descricao = "Fisioterapia" },
    new() { Codigo = "G", Descricao = "Geral" },
    new() { Codigo = "I", Descricao = "Imagiologia" },
    new() { Codigo = "M", Descricao = "MCDT" },
  ];

  public async Task<Response<ExamesSemPapelContextoDTO>> ObterContextoAsync(Guid clinicaId)
  {
    try
    {
      var clinica = await _repository.GetByIdAsync<Clinica, Guid>(clinicaId);
      if (clinica == null)
        return ResponseFactory.Fail<ExamesSemPapelContextoDTO>("Clínica não encontrada.");

      var assinatura = await ObterAssinaturaSessaoAtualAsync();

      return ResponseFactory.Success(new ExamesSemPapelContextoDTO
      {
        PortaLeitorCartoes = clinica.PortaLeitorCartoes,
        TemAssinaturaCarregada = assinatura is not null,
        AreaPrestacaoAssinarESPDefeito = clinica.AreaPrestacaoAssinarESPDefeito,
        PermitirElaborarRelatorioESP = true,
        AreasPrestacao = AreasPrestacao
      });
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<ExamesSemPapelContextoDTO>(ex.Message);
    }
  }

  public async Task<PaginatedResponse<ExameSemPapelTabelaDTO>> ObterTabelaAsync(
    Guid clinicaId,
    ExameSemPapelFiltroRequest request
  )
  {
    var configSpec = new ConfigExamesSemPapelPorClinicaSpec(clinicaId);
    var config = (await _repository.GetListAsync<ConfigExamesSemPapel, Guid>(configSpec)).FirstOrDefault();

    if (config == null)
      return new PaginatedResponse<ExameSemPapelTabelaDTO>([], 0, request.PageNumber, request.PageSize);

    var exames = await _repository.GetListAsync<Exame, Guid>();
    var utentes = await _repository.GetListAsync<Utente, Guid>();
    var medicos = await _repository.GetListAsync<Medico, Guid>();
    var operacoes = await _repository.GetListAsync<ExamesSemPapelOperacao, Guid>();

    var utentesPorId = utentes.ToDictionary(x => x.Id, x => x.Nome ?? string.Empty);
    var medicosPorId = medicos.ToDictionary(x => x.Id, x => x.Nome ?? string.Empty);
    var operacoesPorReq = operacoes
      .Where(x => x.ClinicaId == clinicaId)
      .ToDictionary(x => x.RequisicaoId, x => x, StringComparer.OrdinalIgnoreCase);

    var rows = exames.Select(exame =>
    {
      var requisicaoId = exame.Id.ToString();
      var requisicaoNumero = (exame.NumeroPrescricao ?? string.Empty).Trim();

      operacoesPorReq.TryGetValue(requisicaoId, out var op);

      var assinado = op?.Assinado == true;
      var comunicado = op?.Comunicado == true;
      var estado = comunicado ? "Realizado" : assinado ? "Efetivado" : "Agendado";

      return new ExameSemPapelTabelaDTO
      {
        Id = requisicaoId,
        RequisicaoNum = requisicaoNumero,
        Utente = utentesPorId.TryGetValue(exame.UtenteId, out var nomeUtente) ? nomeUtente : string.Empty,
        Area = op?.AreaPrestacao ?? config.AreaPrestacao ?? string.Empty,
        Estado = estado,
        Lotes = op?.Lotes == true,
        Medico = medicosPorId.TryGetValue(exame.MedicoId, out var nomeMedico) ? nomeMedico : string.Empty,
        IsencaoTaxa = op?.IsencaoTaxa == true,
        ComTaxa = op?.ComTaxa == true,
        Pnp = op?.Pnp == true,
        Assinado = assinado,
        DataRequisicao = exame.DataPrescricao.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
      };
    }).ToList();

    if (!string.IsNullOrWhiteSpace(request.SearchUtente))
    {
      var term = request.SearchUtente.Trim();
      rows = rows
        .Where(x => x.Utente.Contains(term, StringComparison.OrdinalIgnoreCase))
        .ToList();
    }

    if (request.DataInicio.HasValue)
    {
      var dataInicio = request.DataInicio.Value.Date;
      rows = rows
        .Where(x =>
        {
          _ = DateTime.TryParseExact(
            x.DataRequisicao,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dataReq
          );
          return dataReq.Date >= dataInicio;
        })
        .ToList();
    }

    if (request.DataFim.HasValue)
    {
      var dataFim = request.DataFim.Value.Date;
      rows = rows
        .Where(x =>
        {
          _ = DateTime.TryParseExact(
            x.DataRequisicao,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dataReq
          );
          return dataReq.Date <= dataFim;
        })
        .ToList();
    }

    if (request.PorAssinar)
      rows = rows.Where(x => !x.Assinado).ToList();

    if (request.ApenasEfetuadosNaoPrescritos)
      rows = rows.Where(x => string.Equals(x.Estado, "Efetivado", StringComparison.OrdinalIgnoreCase)).ToList();

    var total = rows.Count;
    var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
    var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
    var paged = rows
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToList();

    return new PaginatedResponse<ExameSemPapelTabelaDTO>(paged, total, pageNumber, pageSize);
  }

  public async Task<Response<bool>> GuardarAssinaturaSessaoAsync(ExameSemPapelAssinaturaSessaoRequest request)
  {
    try
    {
      if (!Guid.TryParse(_currentTenantUserService.UserId, out var utilizadorId))
        return ResponseFactory.Fail<bool>("Utilizador atual inválido.");

      if (string.IsNullOrWhiteSpace(request.DigestValue)
          || string.IsNullOrWhiteSpace(request.SignatureValue)
          || string.IsNullOrWhiteSpace(request.Assinatura)
          || string.IsNullOrWhiteSpace(request.AssinaturaSubCA))
        return ResponseFactory.Fail<bool>("Dados de assinatura incompletos.");

      var assinatura = await ObterAssinaturaSessaoAtualAsync();
      if (assinatura == null)
      {
        assinatura = new ExamesSemPapelAssinaturaSessao
        {
          UtilizadorId = utilizadorId
        };

        await _repository.CreateAsync<ExamesSemPapelAssinaturaSessao, Guid>(assinatura);
      }

      assinatura.CMedico = request.CMedico.Trim();
      assinatura.TipoCartao = request.TipoCartao.Trim();
      assinatura.DigestValue = request.DigestValue.Trim();
      assinatura.SignatureValue = request.SignatureValue.Trim();
      assinatura.Assinatura = request.Assinatura.Trim();
      assinatura.AssinaturaSubCA = request.AssinaturaSubCA.Trim();
      assinatura.AtualizadoEmUtc = DateTime.UtcNow;

      _ = await _repository.UpdateAsync<ExamesSemPapelAssinaturaSessao, Guid>(assinatura);
      await _repository.SaveChangesAsync();

      return ResponseFactory.Success(true);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<bool>(ex.Message);
    }
  }

  public async Task<Response<bool>> LimparAssinaturaSessaoAsync()
  {
    try
    {
      var assinatura = await ObterAssinaturaSessaoAtualAsync();
      if (assinatura == null)
        return ResponseFactory.Success(true);

      _ = await _repository.RemoveByIdAsync<ExamesSemPapelAssinaturaSessao, Guid>(assinatura.Id);
      await _repository.SaveChangesAsync();

      return ResponseFactory.Success(true);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<bool>(ex.Message);
    }
  }

  public async Task<Response<ExameSemPapelLoteResultadoDTO>> AssinarLoteAsync(
    Guid clinicaId,
    ExameSemPapelLoteRequest request
  )
  {
    _ = clinicaId;

    if (request.Requisicoes.Count == 0)
      return ResponseFactory.Fail<ExameSemPapelLoteResultadoDTO>("Selecione pelo menos uma requisição.");

    var assinatura = await ObterAssinaturaSessaoAtualAsync();
    if (assinatura == null)
      return ResponseFactory.Fail<ExameSemPapelLoteResultadoDTO>("Assinatura digital não carregada.");

    var operacoes = await UpsertOperacoesAsync(clinicaId, request.Requisicoes);
    foreach (var op in operacoes)
    {
      op.Assinado = true;
      op.Lotes = true;
      if (!string.IsNullOrWhiteSpace(request.AreaPrestacao))
        op.AreaPrestacao = request.AreaPrestacao.Trim();
      op.UltimaOperacaoUtc = DateTime.UtcNow;
      _ = await _repository.UpdateAsync<ExamesSemPapelOperacao, Guid>(op);
    }

    await _repository.SaveChangesAsync();

    var resultado = new ExameSemPapelLoteResultadoDTO
    {
      Total = request.Requisicoes.Count,
      Sucesso = request.Requisicoes.Count,
      Falha = 0,
      Mensagens = [$"Assinadas {request.Requisicoes.Count} requisições em lote."]
    };

    return ResponseFactory.Success(resultado);
  }

  public async Task<Response<ExameSemPapelLoteResultadoDTO>> ComunicarLoteAsync(
    Guid clinicaId,
    ExameSemPapelLoteRequest request
  )
  {
    _ = clinicaId;

    if (request.Requisicoes.Count == 0)
      return ResponseFactory.Fail<ExameSemPapelLoteResultadoDTO>("Selecione pelo menos uma requisição.");

    var assinatura = await ObterAssinaturaSessaoAtualAsync();
    if (assinatura == null)
      return ResponseFactory.Fail<ExameSemPapelLoteResultadoDTO>("Assinatura digital não carregada.");

    var operacoes = await UpsertOperacoesAsync(clinicaId, request.Requisicoes);
    var resultado = new ExameSemPapelLoteResultadoDTO
    {
      Total = request.Requisicoes.Count,
      Sucesso = 0,
      Falha = 0
    };

    foreach (var op in operacoes)
    {
      if (!op.Assinado)
      {
        resultado.Falha++;
        resultado.Mensagens.Add($"Requisição {op.RequisicaoId} não assinada.");
        continue;
      }

      op.Comunicado = true;
      op.Lotes = true;
      if (!string.IsNullOrWhiteSpace(request.AreaPrestacao))
        op.AreaPrestacao = request.AreaPrestacao.Trim();
      op.UltimaOperacaoUtc = DateTime.UtcNow;
      _ = await _repository.UpdateAsync<ExamesSemPapelOperacao, Guid>(op);
      resultado.Sucesso++;
    }

    await _repository.SaveChangesAsync();

    if (resultado.Sucesso > 0 && resultado.Falha == 0)
      resultado.Mensagens.Add($"Comunicadas {resultado.Sucesso} requisições em lote.");
    else if (resultado.Sucesso > 0)
      resultado.Mensagens.Add($"Comunicadas {resultado.Sucesso} de {resultado.Total} requisições.");

    return ResponseFactory.Success(resultado);
  }

  private async Task<List<ExamesSemPapelOperacao>> UpsertOperacoesAsync(
    Guid clinicaId,
    IEnumerable<string> requisicoes
  )
  {
    var requisicoesNormalizadas = requisicoes
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(x => x.Trim())
      .Distinct(StringComparer.OrdinalIgnoreCase)
      .ToList();

    var existentes = (await _repository.GetListAsync<ExamesSemPapelOperacao, Guid>())
      .Where(x => x.ClinicaId == clinicaId && requisicoesNormalizadas.Contains(x.RequisicaoId))
      .ToList();

    foreach (var req in requisicoesNormalizadas)
    {
      if (existentes.Any(x => string.Equals(x.RequisicaoId, req, StringComparison.OrdinalIgnoreCase)))
        continue;

      var nova = new ExamesSemPapelOperacao
      {
        ClinicaId = clinicaId,
        RequisicaoId = req,
        UltimaOperacaoUtc = DateTime.UtcNow
      };

      await _repository.CreateAsync<ExamesSemPapelOperacao, Guid>(nova);
      existentes.Add(nova);
    }

    return existentes;
  }

  private async Task<ExamesSemPapelAssinaturaSessao?> ObterAssinaturaSessaoAtualAsync()
  {
    if (!Guid.TryParse(_currentTenantUserService.UserId, out var utilizadorId))
      return null;

    var spec = new ExamesSemPapelAssinaturaSessaoPorUtilizadorSpec(utilizadorId);
    return (await _repository.GetListAsync<ExamesSemPapelAssinaturaSessao, Guid>(spec)).FirstOrDefault();
  }
}
