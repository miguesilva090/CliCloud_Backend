using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.Filters
{
  public class TipoServicoTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

