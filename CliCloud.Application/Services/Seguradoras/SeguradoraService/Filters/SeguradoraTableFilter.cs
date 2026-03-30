using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.Filters
{
  public class SeguradoraTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
