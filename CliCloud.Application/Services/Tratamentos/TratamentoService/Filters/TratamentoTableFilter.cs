using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.Filters
{
  public class TratamentoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

