using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService;
using CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public class AdmissaoAdministrativoService(
  IRepositoryAsync repository,
  IMapper mapper,
  IRequisicaoEspFechoUpdater requisicaoEspFechoUpdater,
  IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver
) : IAdmissaoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IMapper _mapper = mapper;
  private readonly IRequisicaoEspFechoUpdater _requisicaoEspFechoUpdater = requisicaoEspFechoUpdater;
  private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
    utilizadorDisplayNameResolver;

  public async Task<PaginatedResponse<AdmissaoTableDTO>> GetPaginatedAsync(AdmissaoTableFilter filter)
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    DateTime dataRef = (filter.DataReferencia ?? DateTime.UtcNow).Date;
    var spec = new AdmissaoSearchTable(filter.Modo, dataRef, filter.Filters ?? [], order);
    PaginatedResponse<AdmissaoTableDTO> result =
      await _repository.GetPaginatedResultsAsync<Admissao, AdmissaoTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );

    await HydrateUtenteNumerosAsync(result.Data);
    return result;
  }

  public async Task<Response<AdmissaoDTO>> GetByIdAsync(Guid id)
  {
    List<Admissao> list = (await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))).ToList();
    Admissao? entity = list.FirstOrDefault();
    if (entity == null)
    {
      return ResponseFactory.Fail<AdmissaoDTO>("Admissão não encontrada.");
    }

    AdmissaoDTO dto = _mapper.Map<AdmissaoDTO>(entity);
    await HydrateUtenteNumeroAsync(dto, entity.UtenteId);
    return ResponseFactory.Success(dto);
  }

  public async Task<Response<Guid>> CreateAsync(CreateAdmissaoRequest request)
  {
    if (request.ConsultaMarcacaoId.HasValue)
    {
      List<Admissao> existentes = (await _repository.GetListAsync<Admissao, Guid>()).ToList();
      if (existentes.Any(a =>
            a.ConsultaMarcacaoId == request.ConsultaMarcacaoId && a.DeletedOn == null))
      {
        return ResponseFactory.Fail<Guid>("Já existe uma admissão para esta marcação.");
      }
    }

    Admissao entity = _mapper.Map<Admissao>(request);
    entity.Id = Guid.NewGuid();
    entity.Pago = false;
    entity.Faturado = false;

    if (request.ConsultaMarcacaoId.HasValue)
    {
      ConsultaMarcacao? marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        request.ConsultaMarcacaoId.Value
      );
      if (marcacao != null)
      {
        entity.EmTratamento = marcacao.EmTratamento;
      }
    }

    NormalizeServicos(entity);
    await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(entity, _repository);
    _ = await _repository.CreateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> UpdateAsync(Guid id, UpdateAdmissaoRequest request)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    bool? pago = entity.Pago;
    bool? faturado = entity.Faturado;
    string? obs = entity.Obs;
    _mapper.Map(request, entity);
    entity.Pago = pago;
    entity.Faturado = faturado;
    entity.Obs = obs;
    NormalizeServicos(entity);
    await AdmissaoHoraCalculoHelper.AplicarHoraFimAsync(entity, _repository);
    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> DeleteAsync(Guid id)
  {
    await _repository.RemoveByIdAsync<Admissao, Guid>(id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> ConfirmarAsync(Guid id, bool confirmado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.StatusConsulta
        is StatusConsulta.Faltou
        or StatusConsulta.FaltouJustificada)
    {
      return ResponseFactory.Fail<Guid>("Não é possível alterar a presença de uma admissão com falta.");
    }

    entity.Confirmado = confirmado;
    if (confirmado)
    {
      entity.HoraChegada = DateTime.Now.TimeOfDay;
      entity.Ordem ??= await AdmissaoOrdemHelper.ObterProximaOrdemDiaAsync(entity, _repository);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetConfirmaConsultaAsync(Guid id, bool confirmaConsulta)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if (entity.StatusConsulta
        is StatusConsulta.Faltou
        or StatusConsulta.FaltouJustificada)
    {
      return ResponseFactory.Fail<Guid>("Não é possível alterar a confirmação de uma admissão com falta.");
    }

    entity.ConfirmaConsulta = confirmaConsulta;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetEmTratamentoAsync(Guid id, bool emTratamento)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.EmTratamento = emTratamento;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> SetEfetuadoAsync(Guid id, bool efetuado)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    entity.Efetuado = efetuado;
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> DesmarcarAsync(Guid id, DesmarcarAdmissaoRequest request)
  {
    if (string.IsNullOrWhiteSpace(request.Motivo))
    {
      return ResponseFactory.Fail<Guid>("Indique o motivo da desmarcação.");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);

    if (entity.Pago == true || entity.Faturado == true)
    {
      return ResponseFactory.Fail<Guid>(
        "A admissão tem recibo ou fatura associados e não pode ser desmarcada."
      );
    }

    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    string linhaMotivo =
      $"{nomeAutor} - {DateTime.Now:dd-MM-yyyy HH:mm}{Environment.NewLine}{request.Motivo!.Trim()}";
    entity.Obs = string.IsNullOrWhiteSpace(entity.Obs)
      ? linhaMotivo
      : $"{linhaMotivo}{Environment.NewLine}{Environment.NewLine}{entity.Obs}";

    entity.StatusConsulta = StatusConsulta.Desmarcada;
    entity.DataHoraMarcacao = DateTime.UtcNow;
    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<PromoverAdmissaoResultDTO>> PromoverParaConsultaAsync(Guid id)
  {
    List<Admissao> list = (
      await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))
    ).ToList();
    Admissao? admissao = list.FirstOrDefault();
    if (admissao == null)
    {
      return ResponseFactory.Fail<PromoverAdmissaoResultDTO>("Admissão não encontrada.");
    }

    bool sugerirMarcacoesFisio = admissao.TipoAdmissao?.CodigoLegado == 1;

    try
    {
      Guid consultaId = await AdmissaoPromocaoRunner.PromoverAsync(
        admissao,
        _repository,
        _requisicaoEspFechoUpdater
      );
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(
        new PromoverAdmissaoResultDTO
        {
          ConsultaId = consultaId,
          SugerirMarcacoesFisio = sugerirMarcacoesFisio,
        }
      );
    }
    catch (InvalidOperationException ex)
    {
      return ResponseFactory.Fail<PromoverAdmissaoResultDTO>(ex.Message);
    }
  }

  public async Task<Response<FechoDiarioResultDTO>> PromoverLoteAsync(PromoverAdmissaoLoteRequest request)
  {
    if (request.Ids == null || request.Ids.Count == 0)
    {
      return ResponseFactory.Fail<FechoDiarioResultDTO>("Selecione pelo menos uma admissão.");
    }

    var result = new FechoDiarioResultDTO { TotalElegiveis = request.Ids.Count };
    HashSet<Guid> ids = request.Ids.Distinct().ToHashSet();

    HashSet<Guid> idsJaPromovidas = (
      await _repository.GetListAsync<Consulta, Guid>(new ConsultasPromovidasPorAdmissoesSpec(ids))
    )
      .Where(c => c.AdmissaoId.HasValue)
      .Select(c => c.AdmissaoId!.Value)
      .ToHashSet();

    try
    {
      foreach (Guid id in ids)
      {
        result.TotalProcessadas++;

        if (idsJaPromovidas.Contains(id))
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Admissão {id}: já promovida (ignorada).");
          continue;
        }

        List<Admissao> list = (
          await _repository.GetListAsync<Admissao, Guid>(new AdmissaoByIdWithServicosSpec(id))
        ).ToList();
        Admissao? admissao = list.FirstOrDefault();
        if (admissao == null)
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Admissão {id}: não encontrada.");
          continue;
        }

        if (admissao.StatusConsulta == StatusConsulta.Desmarcada)
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Admissão {id}: desmarcada (ignorada).");
          continue;
        }

        _ = await AdmissaoPromocaoRunner.PromoverAsync(
          admissao,
          _repository,
          _requisicaoEspFechoUpdater
        );
        result.TotalConsultasCriadas++;
        idsJaPromovidas.Add(id);
      }

      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(result);
    }
    catch (Exception ex)
    {
      _repository.ClearChangeTracker();
      return ResponseFactory.Fail<FechoDiarioResultDTO>($"Promoção em lote cancelada: {ex.Message}");
    }
  }

  public async Task<Response<AdmissaoObservacoesDTO>> GetObservacoesAsync(Guid id)
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    return ResponseFactory.Success(
      new AdmissaoObservacoesDTO { Observacoes = entity.Obs ?? string.Empty }
    );
  }

  public async Task<Response<Guid>> AppendObservacaoAsync(
    Guid id,
    AppendAdmissaoObservacaoRequest request
  )
  {
    if (string.IsNullOrWhiteSpace(request.Texto))
    {
      return ResponseFactory.Fail<Guid>("Indique o texto da observação.");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
      request.Texto,
      nomeAutor,
      entity.Obs
    );

    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<PaginatedResponse<OrdemEntradaTableDTO>> GetOrdemEntradaPaginatedAsync(
    OrdemEntradaTableFilter filter
  )
  {
    if(filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new OrdemEntradaSearchTable(filter, order);
    PaginatedResponse<OrdemEntradaTableDTO> result = 
      await _repository.GetPaginatedResultsAsync<Admissao, OrdemEntradaTableDTO, Guid>(
        filter.PageNumber, 
        filter.PageSize,
        spec
      );

    await HydrateOrdemEntradaUtenteNumerosAsync(result.Data);
    await HydrateOrdemEntradaCreatedByNomesAsync(result.Data);
    HydrateOrdemEntradaConsultaPromovida(result.Data);
    ApplyOrdemEntradaStatusLabels(result.Data);
    return result;
  }

  public async Task<Response<Guid>> DefinirOrdemEntradaAsync(
    Guid id, 
    DefinirOrdemEntradaRequest request
  )
  {
    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if(entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Admissão não encontrada.");
    }

    if(entity.StatusConsulta == StatusConsulta.Desmarcada)
    {
      return ResponseFactory.Fail<Guid>("Não é possível alterar a ordem de uma admissão anulada");
    }

    entity.Ordem = request.Ordem;
    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  public async Task<Response<Guid>> AnularOrdemEntradaAsync(
    Guid id, 
    AnularOrdemEntradaRequest request
  )
  {
    if(string.IsNullOrWhiteSpace(request.Motivo))
    {
      return ResponseFactory.Fail<Guid>("Indique o motivo da anulação");
    }

    Admissao entity = await _repository.GetByIdAsync<Admissao, Guid>(id);
    if(entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<Guid>("Admissão não encontrada");
    }
    if(entity.Pago == true || entity.Faturado == true)
    {
      return ResponseFactory.Fail<Guid>("A consulta tem recibo ou fatura associados e não pode ser anulada");
    }

    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    string linhaMotivo = 
      $"{nomeAutor} - {DateTime.Now:dd-MM-yyyy HH:mm}{Environment.NewLine}{request.Motivo!.Trim()}";
    entity.Obs = string.IsNullOrWhiteSpace(entity.Obs)
      ? linhaMotivo
      : $"{linhaMotivo}{Environment.NewLine}{Environment.NewLine}{entity.Obs}";

    entity.StatusConsulta = StatusConsulta.Desmarcada;
    entity.DataHoraMarcacao = DateTime.UtcNow;
    entity.Confirmado = false;

    _ = await _repository.UpdateAsync<Admissao, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  private async Task HydrateOrdemEntradaUtenteNumerosAsync(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if(ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    foreach(OrdemEntradaTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if(u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateOrdemEntradaCreatedByNomesAsync(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    List<Guid> ids = rows.Select(r => r.CreatedBy).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    IReadOnlyDictionary<Guid, string> nomes =
      await _utilizadorDisplayNameResolver.ResolveManyByIdsAsync(ids);

    foreach (OrdemEntradaTableDTO row in rows)
    {
      if (nomes.TryGetValue(row.CreatedBy, out string? nome))
      {
        row.CreatedByNome = nome;
      }
    }
  }

  private static void HydrateOrdemEntradaConsultaPromovida(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    foreach(OrdemEntradaTableDTO row in rows)
    {
      if(row.ConsultaId.HasValue)
      {
        row.ConsultaPromovida = true;
      }
    }
  }

  private static void ApplyOrdemEntradaStatusLabels(
    IReadOnlyCollection<OrdemEntradaTableDTO> rows
  )
  {
    foreach(OrdemEntradaTableDTO row in rows)
    {
      row.StatusConsultaLabel = row.StatusConsulta?.ToString();
    }
  }

  private static void NormalizeServicos(Admissao entity)
  {
    int linha = 1;
    foreach (AdmissaoServico servico in entity.Servicos.OrderBy(s => s.Linha))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      servico.AdmissaoId = entity.Id;
      if (servico.Linha <= 0)
      {
        servico.Linha = linha;
      }

      linha++;
    }
  }

  private async Task HydrateUtenteNumerosAsync(IReadOnlyCollection<AdmissaoTableDTO> rows)
  {
    List<Guid> ids = rows.Select(r => r.UtenteId).Distinct().ToList();
    if (ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    foreach (AdmissaoTableDTO row in rows)
    {
      UtenteNumeroLookupDTO? u = lookups.FirstOrDefault(x => x.Id == row.UtenteId);
      if (u != null)
      {
        row.UtenteNumero = u.NumeroUtente;
      }
    }
  }

  private async Task HydrateUtenteNumeroAsync(AdmissaoDTO dto, Guid utenteId)
  {
    var spec = new UtenteNumerosByIdsSpecification([utenteId]);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();
    UtenteNumeroLookupDTO? u = lookups.FirstOrDefault();
    if (u != null)
    {
      dto.UtenteNumero = u.NumeroUtente;
    }
  }
}
