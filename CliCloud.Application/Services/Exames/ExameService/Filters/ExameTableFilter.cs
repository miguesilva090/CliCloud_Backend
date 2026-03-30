using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Exames.ExameService.Filters
{
  public class ExameTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
