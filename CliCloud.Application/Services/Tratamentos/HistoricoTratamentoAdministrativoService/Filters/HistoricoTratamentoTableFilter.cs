using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;

public class HistoricoTratamentoTableFilter : PaginationFilter
{
  public string Modo { get; set; } = HistoricoTratamentoAdministrativoModos.Datas;
  public List<TableFilter> Filters { get; set; } = [];

  public Guid? UtenteId { get; set; }
  public Guid? FisioterapeutaId { get; set; }
  public Guid? AuxiliarId { get; set; }
  public Guid? OutroTecnicoId { get; set; }
  public Guid? OrganismoId { get; set; }
}
