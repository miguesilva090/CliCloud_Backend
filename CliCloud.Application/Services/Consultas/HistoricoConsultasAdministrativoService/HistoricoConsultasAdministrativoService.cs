using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;

public class HistoricoConsultasAdministrativoService : IHistoricoConsultasAdministrativoService
{
  private readonly IRepositoryAsync _repository;
  private readonly IMapper _mapper;

  public HistoricoConsultasAdministrativoService(IRepositoryAsync repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<PaginatedResponse<HistoricoConsultaAdministrativoRowDTO>> GetPaginatedAsync(
    HistoricoConsultaAdministrativoTableFilter filter
  )
  {
    string? vista = (filter.Vista ?? string.Empty).Trim().ToLowerInvariant();
    if (!HistoricoConsultaAdministrativoVistas.IsValid(vista))
    {
      vista = HistoricoConsultaAdministrativoVistas.Datas;
    }

    filter.Vista = vista;

    if (filter.Filters is { Count: > 0 })
    {
      filter.PageNumber = 1;
    }

    string order =
      filter.Sorting is { Count: > 0 } ? GSHelpers.GenerateOrderByString(filter) : string.Empty;

    if (string.IsNullOrWhiteSpace(order))
    {
      order = vista switch
      {
        HistoricoConsultaAdministrativoVistas.Utentes => "Utente.Nome,-Data",
        HistoricoConsultaAdministrativoVistas.Medicos => "Medico.Nome,-Data",
        HistoricoConsultaAdministrativoVistas.Organismos => "Organismo.Nome,-Data",
        _ => "-Data",
      };
    }

    var spec = new HistoricoConsultaAdministrativoSearchSpec(filter.Filters ?? [], order);
    PaginatedResponse<HistoricoConsultaAdministrativoRowDTO> result =
      await _repository.GetPaginatedResultsAsync<Consulta, HistoricoConsultaAdministrativoRowDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );

    await HydrateUtenteNumerosAsync(result.Data);
    await HydrateFaturacaoAsync(result.Data);
    return result;
  }

  public async Task<Response<AdmissaoDTO>> GetConsultaHistoricoForEditAsync(Guid consultaId)
  {
    try
    {
      AdmissaoDTO dto = await _repository.GetByIdAsync<Consulta, AdmissaoDTO, Guid>(
        consultaId,
        new ConsultaHistoricoAdministrativoSpec(consultaId));

      dto.Servicos = (
        await _repository.GetListAsync<ServicoConsulta, AdmissaoServicoDTO, Guid>(
          new ServicosConsultaPorConsultaIdSpec(consultaId))
      ).ToList();

      await HydratePagoFaturadoConsultaAsync(dto, consultaId);
      return ResponseFactory.Success(dto);
    }
    catch (InvalidOperationException)
    {
      return ResponseFactory.Fail<AdmissaoDTO>("Consulta não encontrada.");
    }
  }

  public async Task<Response<Guid>> UpdateConsultaHistoricoAsync(
    Guid consultaId,
    UpdateConsultaHistoricoRequest request
  )
  {
    Consulta entity;
    try
    {
      entity = await _repository.GetByIdAsync<Consulta, Guid>(consultaId);
    }
    catch (InvalidOperationException)
    {
      return ResponseFactory.Fail<Guid>("Consulta não encontrada.");
    }

    _mapper.Map(request, entity);
    AplicarEstadosConsulta(entity, request);
    await ReplaceServicosConsultaAsync(entity, request.Servicos ?? []);
    NormalizeServicosConsulta(entity);
    _ = await _repository.UpdateAsync<Consulta, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  private static void AplicarEstadosConsulta(Consulta entity, UpdateConsultaHistoricoRequest request)
  {
    entity.Confirmado = request.Confirmado;
    entity.Efetuado = request.Efetuado;

    if (request.StatusConsulta.HasValue)
    {
      entity.StatusConsulta = request.StatusConsulta;
      if (request.StatusConsulta
          is StatusConsulta.Faltou
          or StatusConsulta.FaltouJustificada)
      {
        entity.Faltou = true;
      }
      else if (request.StatusConsulta
               is StatusConsulta.Desmarcada
               or StatusConsulta.Suspensa)
      {
        entity.Faltou = false;
      }
    }
    else if (request.Efetuado == true)
    {
      entity.StatusConsulta = StatusConsulta.Concluida;
    }
  }

  private async Task HydrateUtenteNumerosAsync(IReadOnlyCollection<HistoricoConsultaAdministrativoRowDTO> rows)
  {
    List<Guid> ids = rows
      .Where(r => r.UtenteId.HasValue)
      .Select(r => r.UtenteId!.Value)
      .Distinct()
      .ToList();
    if (ids.Count == 0)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification(ids);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();
    Dictionary<Guid, string?> dict = lookups.ToDictionary(x => x.Id, x => x.NumeroUtente);
    foreach (HistoricoConsultaAdministrativoRowDTO row in rows)
    {
      if (row.UtenteId.HasValue && dict.TryGetValue(row.UtenteId.Value, out string? numero))
      {
        row.UtenteNumero = numero;
      }
    }
  }

  private async Task HydrateFaturacaoAsync(IReadOnlyCollection<HistoricoConsultaAdministrativoRowDTO> rows)
  {
    if (rows.Count == 0)
    {
      return;
    }

    List<Guid> ids = rows.Select(r => r.Id).Distinct().ToList();
    var spec = new ConsultaFaturacaoByConsultaIdsListSpec(ids);
    List<ConsultaFaturacao> fatRows = (await _repository.GetListAsync<ConsultaFaturacao, Guid>(spec)).ToList();

    Dictionary<Guid, (bool Pago, bool Faturado)> agg = fatRows
      .Where(x => x.ConsultaId != null)
      .GroupBy(x => x.ConsultaId!.Value)
      .ToDictionary(
        g => g.Key,
        g => (g.Any(x => x.Pago), g.Any(x => x.Faturado)));

    foreach (HistoricoConsultaAdministrativoRowDTO row in rows)
    {
      if (agg.TryGetValue(row.Id, out (bool Pago, bool Faturado) v))
      {
        row.Pago = v.Pago;
        row.Faturado = v.Faturado;
      }
    }
  }

  private static void HydrateDisplayNamesFromEntity(AdmissaoDTO dto, Consulta entity)
  {
    dto.UtenteNome ??= entity.Utente?.Nome;
    dto.MedicoNome ??= entity.Medico?.Nome;
    dto.OrganismoNome ??= entity.Organismo?.Nome;
    dto.SalaNome ??= entity.Sala?.Nome;
    dto.EspecialidadeNome ??= entity.Especialidade?.Nome;
    dto.MedicoExternoNome ??= entity.MedicoExterno?.Nome;
  }

  private async Task HydrateUtenteNumeroAsync(AdmissaoDTO dto, Guid? utenteId)
  {
    if (!utenteId.HasValue)
    {
      return;
    }

    var spec = new UtenteNumerosByIdsSpecification([utenteId.Value]);
    List<UtenteNumeroLookupDTO> lookups = (
      await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
    ).ToList();

    UtenteNumeroLookupDTO? lookup = lookups.FirstOrDefault();
    if (lookup != null)
    {
      dto.UtenteNumero = lookup.NumeroUtente;
    }
  }

  private async Task HydratePagoFaturadoConsultaAsync(AdmissaoDTO dto, Guid consultaId)
  {
    var spec = new ConsultaFaturacaoByConsultaId(consultaId);
    List<ConsultaFaturacao> fatRows = (
      await _repository.GetListAsync<ConsultaFaturacao, Guid>(spec)
    ).ToList();
    if (fatRows.Count == 0)
    {
      return;
    }

    dto.Pago = fatRows.Any(x => x.Pago);
    dto.Faturado = fatRows.Any(x => x.Faturado);
  }

  private async Task ReplaceServicosConsultaAsync(Consulta entity, IReadOnlyList<AdmissaoServicoDTO> linhas)
  {
    List<ServicoConsulta> existentes = (
      await _repository.GetListAsync<ServicoConsulta, Guid>(new ServicosConsultaPorConsultaIdSpec(entity.Id))
    ).ToList();

    foreach (ServicoConsulta s in existentes)
    {
      await _repository.RemoveAsync<ServicoConsulta, Guid>(s);
    }

    entity.Servicos.Clear();
    foreach (AdmissaoServicoDTO dto in linhas)
    {
      ServicoConsulta sc = _mapper.Map<ServicoConsulta>(dto);
      sc.Id = sc.Id == Guid.Empty ? Guid.NewGuid() : sc.Id;
      sc.ConsultaId = entity.Id;
      sc.Consulta = entity;
      entity.Servicos.Add(sc);
      _ = await _repository.CreateAsync<ServicoConsulta, Guid>(sc);
    }
  }

  private static void NormalizeServicosConsulta(Consulta entity)
  {
    int linha = 1;
    foreach (ServicoConsulta servico in entity.Servicos.OrderBy(s => s.Linha))
    {
      if (servico.Id == Guid.Empty)
      {
        servico.Id = Guid.NewGuid();
      }

      servico.ConsultaId = entity.Id;
      if (servico.Linha <= 0)
      {
        servico.Linha = linha;
      }

      linha++;
    }
  }
}
