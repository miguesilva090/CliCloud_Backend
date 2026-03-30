using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Filters
{
  public class CartaConducaoRestricoesTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
