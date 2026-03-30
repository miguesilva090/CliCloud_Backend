using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.ReciboService.Filters
{
  public class ReciboTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
