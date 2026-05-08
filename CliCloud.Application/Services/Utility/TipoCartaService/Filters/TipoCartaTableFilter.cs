using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Utility.TipoCartaService.Filters
{
  public class TipoCartaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
