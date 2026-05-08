using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.Filters
{
  public class MotivoConsultaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
