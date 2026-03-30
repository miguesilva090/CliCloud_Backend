using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Filters
{
  public class ServicoSessaoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

