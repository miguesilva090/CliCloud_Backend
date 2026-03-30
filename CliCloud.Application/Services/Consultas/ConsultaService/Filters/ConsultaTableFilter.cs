using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Filters
{
  public class ConsultaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
