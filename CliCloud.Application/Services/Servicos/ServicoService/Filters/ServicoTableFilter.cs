using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Servicos.ServicoService.Filters
{
  public class ServicoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

