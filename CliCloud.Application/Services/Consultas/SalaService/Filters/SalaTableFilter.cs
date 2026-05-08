using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.SalaService.Filters
{
  public class SalaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
