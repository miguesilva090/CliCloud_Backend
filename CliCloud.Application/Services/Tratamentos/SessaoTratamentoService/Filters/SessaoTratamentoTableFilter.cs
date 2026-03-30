using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Filters
{
  public class SessaoTratamentoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

