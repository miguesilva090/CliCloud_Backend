using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;
using CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService;

public class HistoricoConsultasAdministrativoService : IHistoricoConsultasAdministrativoService
{
  private readonly IRepositoryAsync _repository;

  public HistoricoConsultasAdministrativoService(IRepositoryAsync repository)
  {
    _repository = repository;
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
}
