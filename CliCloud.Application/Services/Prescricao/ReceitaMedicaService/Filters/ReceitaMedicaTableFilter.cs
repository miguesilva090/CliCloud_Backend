using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Filters
{
  public class ReceitaMedicaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
