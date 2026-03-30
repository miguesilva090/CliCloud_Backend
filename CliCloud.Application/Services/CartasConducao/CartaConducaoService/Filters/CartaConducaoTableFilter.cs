using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.Filters
{
  public class CartaConducaoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
