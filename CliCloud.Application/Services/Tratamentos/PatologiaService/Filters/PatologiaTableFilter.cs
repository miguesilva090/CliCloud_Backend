using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.Filters
{
  public class PatologiaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
