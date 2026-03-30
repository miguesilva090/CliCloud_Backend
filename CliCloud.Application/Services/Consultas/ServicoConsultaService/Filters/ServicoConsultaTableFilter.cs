using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.Filters
{
  public class ServicoConsultaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

