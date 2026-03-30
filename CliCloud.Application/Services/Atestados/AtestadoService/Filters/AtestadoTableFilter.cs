using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Filters
{
  public class AtestadoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
