using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Core.ClinicaService.DTOs;
using CliCloud.Application.Services.Core.ClinicaService.Filters;
using CliCloud.Application.Services.Core.ClinicaService.Specifications;
using CliCloud.Infrastructure.Encryption;
using CliCloud.Domain.Entities.Core.Tratamentos;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.TaxasIva;
using Microsoft.AspNetCore.Hosting;

namespace CliCloud.Application.Services.Core.ClinicaService
{
  public class ClinicaService : IClinicaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;
    private readonly IWebHostEnvironment _environment;

    public ClinicaService(
      IRepositoryAsync repository,
      IMapper mapper,
      IEncryptionService encryptionService,
      IWebHostEnvironment environment
    )
    {
      _repository = repository;
      _mapper = mapper;
      _encryptionService = encryptionService;
      _environment = environment;
    }

    private static string? NormalizeClinicaLogoUrl(string? imageUrl)
    {
      if (string.IsNullOrWhiteSpace(imageUrl)) return null;
      if (imageUrl.StartsWith('/')) return imageUrl;

      if (
        imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
        || imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
      )
      {
        try
        {
          var uri = new Uri(imageUrl);
          return uri.AbsolutePath;
        }
        catch
        {
          return null;
        }
      }

      return imageUrl;
    }

    private static bool IsValidClinicaLogoUrl(string? url)
    {
      if (string.IsNullOrWhiteSpace(url)) return true;
      if (url.StartsWith('/')) return true;
      return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private Task DeleteOldClinicaLogoIfNeededAsync(string? oldUrl, string? newUrl)
    {
      if (string.IsNullOrWhiteSpace(oldUrl)) return Task.CompletedTask;
      if (string.Equals(oldUrl, newUrl, StringComparison.OrdinalIgnoreCase)) return Task.CompletedTask;
      if (!oldUrl.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase)) return Task.CompletedTask;

      var relative = oldUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
      var fullPath = Path.Combine(_environment.WebRootPath, relative);
      if (File.Exists(fullPath)) File.Delete(fullPath);

      return Task.CompletedTask;
    }

    private string? EncryptIfPlain(string? value)
    {
      if (string.IsNullOrWhiteSpace(value)) return value;

      try
      {
        _ = _encryptionService.DecryptString(value);
        return value;
      }
      catch
      {
        return _encryptionService.EncryptString(value);
      }
    }

    private string? DecryptIfEncrypted(string? value)
    {
      if (string.IsNullOrWhiteSpace(value)) return value;

      try
      {
        return _encryptionService.DecryptString(value);
      }
      catch
      {
        return value;
      }
    }

    private static void NormalizeFaturacaoFields(Clinica c)
    {
      c.Regrafaturacao = c.Regrafaturacao?.Trim() switch
      {
        "1" => "1",
        "2" => "2",
        _ => "1",
      };

      c.ValorMaxFaturaSimpli ??= 0;
      c.FaturaRecibo ??= 1;
      c.Cab ??= false;
      c.ImprimeTicket ??=false;
      c.TemSaft ??= false;
      c.EmailLink ??= false;

      if (string.IsNullOrWhiteSpace(c.FaturacaoDocumentosImpressao))
        c.FaturacaoDocumentosImpressao = "A4";
    }

    private void DecryptDtoSecrets(ClinicaDTO dto)
    {
      dto.AtUser = DecryptIfEncrypted(dto.AtUser);
      dto.AtPass = DecryptIfEncrypted(dto.AtPass);
    }

    private static bool TryParseHora(string? value, out TimeSpan time)
    {
      return TimeSpan.TryParseExact(value, @"hh\:mm", null, out time);
    }

    private static string? ValidateHorario(UpdateClinicaRequest request)
    {
      if(string.IsNullOrWhiteSpace(request.HoraInicManha))
        return "Hora início manhã é obrigatória";
      
      if(string.IsNullOrWhiteSpace(request.HoraFimTarde))
        return "Hora fim tarde é obrigatória";

      if(!TryParseHora(request.HoraInicManha, out var hiM))
        return "Hora início manhã inválida";

      if(!TryParseHora(request.HoraFimTarde, out var hfT))
        return "Hora fim tarde inválida";

      var comInterrupcao = request.Interrupcao == true;

      if(!comInterrupcao)
      {
        if (hiM > hfT)
          return "Intervalo horário inválido";
        return null;
      }

      if(string.IsNullOrWhiteSpace(request.HoraFimManha))
        return "Hora fim manhã é obrigatória quando há interrupção";
      
      if(string.IsNullOrWhiteSpace(request.HoraInicTarde))
        return "Hora início tarde é obrigatória quando há interrupção";

      if(!TryParseHora(request.HoraFimManha, out var hfM))
        return "Hora fim manhã inválida";

      if(!TryParseHora(request.HoraInicTarde, out var hiT))
        return "Hora início tarde inválida";

      if(hiM > hfM || hfM > hiT || hiT > hfT)
        return "Intervalo horário inválido";

      return null;
    }

    private async Task RunCreateSideEffectsAsync(Clinica clinica)
    {
      await EnsureClinicaIvaConfigurationAsync(clinica.Id, clinica.ZonFisc);
      await EnsureClinicaMotivosIsencaoDefaultsAsync(clinica.Id);
      await EnsureClinicaTiposConsultaDefaultsAsync(clinica.Id);
      await EnsureClinicaArmazemGeralDefaultAsync(clinica.Id, clinica.Nome);
      _ = await _repository.SaveChangesAsync();
    }

    private async Task EnsureClinicaIvaConfigurationAsync(Guid clinicaId, ZonaFiscal? zonFisc)
    {
      var configs = await _repository.GetListAsync<ClinicaConfiguracaoIva, Guid>();
      var config = configs.FirstOrDefault(x => x.ClinicaId == clinicaId && x.Ano == DateTime.UtcNow.Year);
      if (config == null)
      {
        var taxas = (await _repository.GetListAsync<TaxaIva, Guid>())
          .OrderBy(x => x.Taxa)
          .ThenBy(x => x.Descricao)
          .ToList();

        var taxa0 = taxas.FirstOrDefault(x => x.Taxa == 0)?.Id;
        var baseSet = taxas.Where(x => x.Taxa > 0).Select(x => x.Id).Take(3).ToList();
        while (baseSet.Count < 3) baseSet.Add(Guid.Empty);

        var novaConfig = new ClinicaConfiguracaoIva
        {
          ClinicaId = clinicaId,
          Ano = DateTime.UtcNow.Year,
          TaxaIva0Id = taxa0,
          TaxaIva1Id = baseSet[0] == Guid.Empty ? null : baseSet[0],
          TaxaIva2Id = baseSet[1] == Guid.Empty ? null : baseSet[1],
          TaxaIva3Id = baseSet[2] == Guid.Empty ? null : baseSet[2],
          TaxaIva4Id = baseSet[0] == Guid.Empty ? null : baseSet[0],
          TaxaIva5Id = baseSet[1] == Guid.Empty ? null : baseSet[1],
          TaxaIva6Id = baseSet[2] == Guid.Empty ? null : baseSet[2],
          TaxaIva7Id = baseSet[0] == Guid.Empty ? null : baseSet[0],
          TaxaIva8Id = baseSet[1] == Guid.Empty ? null : baseSet[1],
          TaxaIva9Id = baseSet[2] == Guid.Empty ? null : baseSet[2],
        };
        config = await _repository.CreateAsync<ClinicaConfiguracaoIva, Guid>(novaConfig);
      }

      if (!zonFisc.HasValue) return;

      // Paridade com legado: muda o grupo de IVA ativo consoante a zona fiscal.
      if (zonFisc == ZonaFiscal.Continente)
      {
        var src = (config.TaxaIva4Id, config.TaxaIva5Id, config.TaxaIva6Id) != (null, null, null)
          ? (config.TaxaIva4Id, config.TaxaIva5Id, config.TaxaIva6Id)
          : (config.TaxaIva7Id, config.TaxaIva8Id, config.TaxaIva9Id) != (null, null, null)
            ? (config.TaxaIva7Id, config.TaxaIva8Id, config.TaxaIva9Id)
            : (config.TaxaIva1Id, config.TaxaIva2Id, config.TaxaIva3Id);
        config.TaxaIva1Id = src.Item1;
        config.TaxaIva2Id = src.Item2;
        config.TaxaIva3Id = src.Item3;
      }
      else if (zonFisc == ZonaFiscal.Madeira)
      {
        var src = (config.TaxaIva1Id, config.TaxaIva2Id, config.TaxaIva3Id) != (null, null, null)
          ? (config.TaxaIva1Id, config.TaxaIva2Id, config.TaxaIva3Id)
          : (config.TaxaIva4Id, config.TaxaIva5Id, config.TaxaIva6Id) != (null, null, null)
            ? (config.TaxaIva4Id, config.TaxaIva5Id, config.TaxaIva6Id)
            : (config.TaxaIva7Id, config.TaxaIva8Id, config.TaxaIva9Id);
        config.TaxaIva7Id = src.Item1;
        config.TaxaIva8Id = src.Item2;
        config.TaxaIva9Id = src.Item3;
      }
      else if (zonFisc == ZonaFiscal.Acores)
      {
        var src = (config.TaxaIva1Id, config.TaxaIva2Id, config.TaxaIva3Id) != (null, null, null)
          ? (config.TaxaIva1Id, config.TaxaIva2Id, config.TaxaIva3Id)
          : (config.TaxaIva7Id, config.TaxaIva8Id, config.TaxaIva9Id) != (null, null, null)
            ? (config.TaxaIva7Id, config.TaxaIva8Id, config.TaxaIva9Id)
            : (config.TaxaIva4Id, config.TaxaIva5Id, config.TaxaIva6Id);
        config.TaxaIva4Id = src.Item1;
        config.TaxaIva5Id = src.Item2;
        config.TaxaIva6Id = src.Item3;
      }

      _ = await _repository.UpdateAsync<ClinicaConfiguracaoIva, Guid>(config);
    }

    private async Task EnsureClinicaMotivosIsencaoDefaultsAsync(Guid clinicaId)
    {
      var existing = await _repository.GetListAsync<ClinicaMotivoIsencaoDefault, Guid>();
      if (existing.Any(x => x.ClinicaId == clinicaId)) return;

      var globalMotivos = await _repository.GetListAsync<MotivoIsencao, Guid>();
      if (!globalMotivos.Any()) return;

      var novos = globalMotivos.Select(x => new ClinicaMotivoIsencaoDefault
      {
        ClinicaId = clinicaId,
        Codigo = x.Codigo,
        Descricao = x.Descricao
      });
      _ = await _repository.CreateRangeAsync<ClinicaMotivoIsencaoDefault, Guid>(novos);
    }

    private async Task EnsureClinicaTiposConsultaDefaultsAsync(Guid clinicaId)
    {
      var existing = await _repository.GetListAsync<ClinicaTipoConsultaDefault, Guid>();
      if (existing.Any(x => x.ClinicaId == clinicaId)) return;

      var tipos = await _repository.GetListAsync<TipoConsultaItem, Guid>();
      if (!tipos.Any()) return;

      var novos = tipos.Select(x => new ClinicaTipoConsultaDefault
      {
        ClinicaId = clinicaId,
        Designacao = x.Designacao
      });
      _ = await _repository.CreateRangeAsync<ClinicaTipoConsultaDefault, Guid>(novos);
    }

    private async Task EnsureClinicaArmazemGeralDefaultAsync(Guid clinicaId, string clinicaNome)
    {
      var existing = await _repository.GetListAsync<ClinicaArmazemDefault, Guid>();
      if (existing.Any(x => x.ClinicaId == clinicaId)) return;

      _ = await _repository.CreateAsync<ClinicaArmazemDefault, Guid>(new ClinicaArmazemDefault
      {
        ClinicaId = clinicaId,
        ArmazemGeral = true,
        Nome = $"Armazem Geral ({clinicaNome})"
      });
    }

    public async Task<Response<IEnumerable<ClinicaDTO>>> GetClinicaAsync(string keyword = "")
    {
      var spec = new ClinicaSearchList(keyword);
      var list = await _repository.GetListAsync<Clinica, ClinicaDTO, Guid>(spec);
      foreach (var dto in list)
      {
        DecryptDtoSecrets(dto);
      }
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ClinicaLightDTO>>> GetClinicaLightAsync(string keyword = "")
    {
      var spec = new ClinicaSearchList(keyword);
      var list = await _repository.GetListAsync<Clinica, ClinicaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ClinicaTableDTO>> GetClinicaPaginatedAsync(ClinicaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ClinicaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Clinica, ClinicaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ClinicaTableDTO>>> GetAllClinicaAsync(ClinicaAllFilter filter)
    {
      try
      {
        filter ??= new ClinicaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ClinicaSearchTable(filters, order);
        var list = await _repository.GetListAsync<Clinica, ClinicaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<ClinicaTableDTO>>(ex.Message); }
    }

    public async Task<Response<ClinicaDTO>> GetClinicaAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (dto == null) return ResponseFactory.Fail<ClinicaDTO>("Clínica não encontrada.");

        DecryptDtoSecrets(dto);
        dto.ConfiguracaoTratamentos = await GetConfiguracaoTratamentosOrNullAsync(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<ClinicaDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateClinicaAsync(CreateClinicaRequest request)
    {
      var spec = new ClinicaMatchNome(request.Nome);
      if (await _repository.ExistsAsync<Clinica, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Clínica com este nome já existe.");
      var entity = _mapper.Map<Clinica>(request);
      entity.TipoEntidade = EntidadeTipo.Clinica;
      entity.UrlFoto = NormalizeClinicaLogoUrl(entity.UrlFoto);
      if (!IsValidClinicaLogoUrl(entity.UrlFoto))
        return ResponseFactory.Fail<Guid>("URL de foto inválida.");

      // Legacy: ATCUDUser/ATCUDPass são persistidos encriptados.
      entity.AtUser = EncryptIfPlain(entity.AtUser);
      entity.AtPass = EncryptIfPlain(entity.AtPass);

      NormalizeFaturacaoFields(entity);

      try
      {
        var created = await _repository.CreateAsync<Clinica, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        await RunCreateSideEffectsAsync(created);
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateClinicaAsync(UpdateClinicaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Clinica, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Clínica não encontrada.");
      var oldZonaFiscal = existing.ZonFisc;
      var oldUrlFoto = existing.UrlFoto;
      if (existing.Nome != request.Nome)
      {
        var spec = new ClinicaMatchNome(request.Nome);
        if (await _repository.ExistsAsync<Clinica, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe uma clínica com este nome.");
      }

      if(!string.IsNullOrWhiteSpace(request.CodSb))
      {
        var codSb = request.CodSb.Trim();
        if(codSb.Length != 4 || !codSb.All(char.IsDigit))
          return ResponseFactory.Fail<Guid>("O código SB tem de conter 4 digitos numéricos");
      }

      var horarioError = ValidateHorario(request);
      if(!string.IsNullOrWhiteSpace(horarioError))
        return ResponseFactory.Fail<Guid>(horarioError);

      
      _mapper.Map(request, existing);

      existing.Atividade = request.Atividade;
      existing.Regcom = request.Regcom;
      existing.Capsocial = request.Capsocial;
      existing.Cae = request.Cae;
      existing.ZonFisc = request.ZonFisc;
      existing.Tipo = request.Tipo;
      existing.Portaria = request.Portaria;
      existing.DespachoUcc = request.DespachoUcc;
      existing.ObsNotaCredito = request.ObsNotaCredito;
      existing.CMoeda = request.CMoeda;
      existing.TipoEntidade = EntidadeTipo.Clinica;
      existing.UrlFoto = NormalizeClinicaLogoUrl(existing.UrlFoto);
      if (!IsValidClinicaLogoUrl(existing.UrlFoto))
        return ResponseFactory.Fail<Guid>("URL de foto inválida.");

      // Legacy: ATCUDUser/ATCUDPass são persistidos encriptados.
      existing.AtUser = EncryptIfPlain(existing.AtUser);
      existing.AtPass = EncryptIfPlain(existing.AtPass);
      
      NormalizeFaturacaoFields(existing);

      try
      {
        var updated = await _repository.UpdateAsync<Clinica, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        await DeleteOldClinicaLogoIfNeededAsync(oldUrlFoto, existing.UrlFoto);

        if (oldZonaFiscal != request.ZonFisc)
        {
          await EnsureClinicaIvaConfigurationAsync(id, request.ZonFisc);
          _ = await _repository.SaveChangesAsync();
        }

        if (request.GravarConfiguracaoTratamentos == true)
        {
          var tratamentosRequest = new AtualizarConfiguracaoTratamentosRequest
          {
            TipoSrvTratamentos = request.TipoSrvTratamentos,
            AreaPrestacaoDefeitoAreaZ = request.AreaPrestacaoDefeitoAreaZ,
            ControlarAparelhos = request.ControlarAparelhos,

            Segundos = request.Segundos,
            FaltasMax = request.FaltasMax,
            FaltasConsecutivasMax = request.FaltasConsecutivasMax,
            Taxamoderadora = request.Taxamoderadora,
            CredencialExternaAdse = request.CredencialExternaAdse,

            TipoPagamento = request.TipoPagamento,
            AvisoInqueritoSessoesDiarias = request.AvisoInqueritoSessoesDiarias,
          };

          var tratamentosRes =
            await GuardarConfiguracaoTratamentosAsync(id, tratamentosRequest);

          if (tratamentosRes.Status != ResponseStatus.Success)
          {
            var msg =
              tratamentosRes.Messages.TryGetValue("$", out var messages) &&
              messages is { Count: > 0 }
                ? messages.First()
                : "Falha ao guardar configurações de tratamentos.";
            return ResponseFactory.Fail<Guid>(msg);
          }
        }

        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    private async Task<ConfiguracaoTratamentosDTO?> GetConfiguracaoTratamentosOrNullAsync(
      Guid clinicaId
    )
    {
      var spec = new ConfiguracaoTratamentosPorClinicaSpec(clinicaId);
      var lista = await _repository.GetListAsync<ConfiguracaoTratamentos, Guid>(spec);
      var entidade = lista.FirstOrDefault();
      if (entidade == null) return null;

      return new ConfiguracaoTratamentosDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        TipoSrvTratamentos = entidade.TipoSrvTratamentos,
        AreaPrestacaoDefeitoAreaZ = entidade.AreaPrestacaoDefeitoAreaZ,
        ControlarAparelhos = entidade.ControlarAparelhos,
        Segundos = entidade.Segundos,
        FaltasMax = entidade.FaltasMax,
        FaltasConsecutivasMax = entidade.FaltasConsecutivasMax,
        Taxamoderadora = entidade.Taxamoderadora,
        CredencialExternaAdse = entidade.CredencialExternaAdse,
        TipoPagamento = entidade.TipoPagamento,
        AvisoInqueritoSessoesDiarias = entidade.AvisoInqueritoSessoesDiarias
      };
    }

    private async Task<Response<Guid>> GuardarConfiguracaoTratamentosAsync(
      Guid clinicaId,
      AtualizarConfiguracaoTratamentosRequest request
    )
    {
      try
      {
        var spec = new ConfiguracaoTratamentosPorClinicaSpec(clinicaId);
        var lista = await _repository.GetListAsync<ConfiguracaoTratamentos, Guid>(spec);
        var entidade = lista.FirstOrDefault();

        if (entidade == null)
        {
          entidade = new ConfiguracaoTratamentos
          {
            ClinicaId = clinicaId,
          };

          Aplicar(entidade, request);
          var criado = await _repository.CreateAsync<ConfiguracaoTratamentos, Guid>(entidade);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(criado.Id);
        }

        Aplicar(entidade, request);
        var atualizado = await _repository.UpdateAsync<ConfiguracaoTratamentos, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(atualizado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void Aplicar(
      ConfiguracaoTratamentos entidade,
      AtualizarConfiguracaoTratamentosRequest request
    )
    {
      entidade.TipoSrvTratamentos = request.TipoSrvTratamentos;
      entidade.AreaPrestacaoDefeitoAreaZ = request.AreaPrestacaoDefeitoAreaZ;
      entidade.ControlarAparelhos = request.ControlarAparelhos;

      entidade.Segundos = request.Segundos;
      entidade.FaltasMax = request.FaltasMax;
      entidade.FaltasConsecutivasMax = request.FaltasConsecutivasMax;
      entidade.Taxamoderadora = request.Taxamoderadora;
      entidade.CredencialExternaAdse = request.CredencialExternaAdse;

      entidade.TipoPagamento = request.TipoPagamento;
      entidade.AvisoInqueritoSessoesDiarias = request.AvisoInqueritoSessoesDiarias;
    }

    public async Task<Response<Guid>> DeleteClinicaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Clinica, Guid>(id);
      
        if(entity == null)
          return ResponseFactory.Fail<Guid>("Clínica não encontrada");
        
        await DeleteOldClinicaLogoIfNeededAsync(entity.UrlFoto, null);
        var removed = await _repository.RemoveByIdAsync<Clinica, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleClinicaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Clinica, Guid>(id);
          if (e == null) { fail.Add($"Clínica {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Clinica, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch { fail.Add($"Clínica {id}."); _repository.ClearChangeTracker(); }
      }
      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    public async Task<Response<Guid>> SetDefaultClinicaAsync(Guid id, bool porDefeito)
    {
      try
      {
        var existing = await _repository.GetByIdAsync<Clinica, Guid>(id);
        if (existing == null) return ResponseFactory.Fail<Guid>("Clínica não encontrada.");

        // Legacy behavior: limpar todos e, se ativo, marcar apenas a escolhida como pordefeito.
        await _repository.ExecuteSqlRawAsync(
          "UPDATE [Core].[Clinica] SET PorDefeito = 0"
        );

        if (porDefeito)
        {
          await _repository.ExecuteSqlRawAsync(
            "UPDATE [Core].[Clinica] SET PorDefeito = 1 WHERE Id = {0}",
            id
          );
        }

        return ResponseFactory.Success(id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<AvisosClinicaLegacyDTO>> GetAvisosClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<AvisosClinicaLegacyDTO>("Clínica não encontrada.");

        return ResponseFactory.Success(new AvisosClinicaLegacyDTO
        {
          MsgFaltaPagamento = clinica.MsgFaltaPagamento,
          MsgCredenciais = clinica.MsgCredenciais
        });
      }
      catch (Exception ex) { return ResponseFactory.Fail<AvisosClinicaLegacyDTO>(ex.Message); }
    }

    public async Task<Response<int[]>> GetFolgasClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<int[]>("Clínica não encontrada.");

        var folgas = new List<int>(7);
        // Compatibilidade com legado (Dom=1, Seg=2, ..., Sab=7)
        if (clinica.FolgaSeg == true) folgas.Add(2);
        if (clinica.FolgaTer == true) folgas.Add(3);
        if (clinica.FolgaQua == true) folgas.Add(4);
        if (clinica.FolgaQui == true) folgas.Add(5);
        if (clinica.FolgaSex == true) folgas.Add(6);
        if (clinica.FolgaSab == true) folgas.Add(7);
        if (clinica.FolgaDom == true) folgas.Add(1);

        return ResponseFactory.Success(folgas.ToArray());
      }
      catch (Exception ex) { return ResponseFactory.Fail<int[]>(ex.Message); }
    }

    public async Task<Response<int?>> GetPortaCartaoClinicaAsync(Guid id)
    {
      try
      {
        var clinica = await _repository.GetByIdAsync<Clinica, ClinicaDTO, Guid>(id);
        if (clinica == null) return ResponseFactory.Fail<int?>("Clínica não encontrada.");

        return ResponseFactory.Success(clinica.PortaLeitorCartoes);
      }
      catch (Exception ex) { return ResponseFactory.Fail<int?>(ex.Message); }
    }

    public async Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasAutocompleteAsync(string? q)
    {
      try
      {
        var spec = new ClinicaAutocompleteSearchList(q);
        var list = await _repository.GetListAsync<Clinica, Guid>(spec);

        var result = list
          .Where(c => !string.IsNullOrWhiteSpace(c.Nome))
          .Select(c => new AutoCompleteItemDTO
          {
            Value = c.Id.ToString(),
            Label = c.Nome.Trim()
          })
          .ToList();

        return ResponseFactory.Success<IEnumerable<AutoCompleteItemDTO>>(result);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AutoCompleteItemDTO>>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<AutoCompleteItemDTO>>> GetClinicasSelectedAutocompleteAsync(string? q, Guid currentClinicaId)
    {
      try
      {
        var spec = new ClinicaAutocompleteSearchList(q);
        var list = await _repository.GetListAsync<Clinica, Guid>(spec);

        var result = list
          .Where(c => !string.IsNullOrWhiteSpace(c.Nome))
          .Select(c => new AutoCompleteItemDTO
          {
            Value = c.Id.ToString(),
            Label = c.Nome.Trim()
          })
          .ToList();

        var current = await _repository.GetByIdAsync<Clinica, Guid>(currentClinicaId);
        if (current != null && !string.IsNullOrWhiteSpace(current.Nome))
        {
          var curValue = current.Id.ToString();
          if (!result.Any(x => string.Equals(x.Value, curValue, StringComparison.OrdinalIgnoreCase)))
          {
            result.Insert(0, new AutoCompleteItemDTO { Value = curValue, Label = current.Nome.Trim() });
          }
        }

        if (result.Count > 10) result = result.Take(10).ToList();

        return ResponseFactory.Success<IEnumerable<AutoCompleteItemDTO>>(result);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AutoCompleteItemDTO>>(ex.Message);
      }
    }

    public async Task<Response<int>> GetConfiguracaoAnoAtivaAsync(Guid clinicaId)
    {
      try
      {
        var list = await _repository.GetListAsync<ClinicaConfiguracaoIva, Guid>();

        var cfg = list 
          .Where(x => x.ClinicaId == clinicaId)
          .OrderByDescending(x => x.Ano)
          .FirstOrDefault();

        if(cfg == null) 
          return ResponseFactory.Fail<int>("Clínica não tem configuração ativa");

        return ResponseFactory.Success(cfg.Ano);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<int>(ex.Message);
      }
    }
  }
}
