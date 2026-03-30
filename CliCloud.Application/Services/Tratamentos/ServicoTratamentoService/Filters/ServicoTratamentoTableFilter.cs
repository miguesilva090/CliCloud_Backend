using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Filters
{
  public class ServicoTratamentoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

