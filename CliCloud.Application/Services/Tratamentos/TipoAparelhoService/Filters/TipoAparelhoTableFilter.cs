using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Filters
{
  public class TipoAparelhoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
