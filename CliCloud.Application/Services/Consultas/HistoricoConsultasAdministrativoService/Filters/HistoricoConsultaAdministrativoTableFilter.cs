using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.HistoricoConsultasAdministrativoService.Filters;

public class HistoricoConsultaAdministrativoTableFilter : PaginationFilter
{
  /// <summary>datas | utentes | medicos | organismos</summary>
  public string Vista { get; set; } = "datas";

  public List<TableFilter> Filters { get; set; } = [];
}
