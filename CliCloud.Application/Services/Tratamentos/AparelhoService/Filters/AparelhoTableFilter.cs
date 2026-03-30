using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.Filters
{
  public class AparelhoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
