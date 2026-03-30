using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Filters
{
  public class MarcacaoConsultaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}

