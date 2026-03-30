using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Core.ClinicaService.Filters
{
  public class ClinicaTableFilter : PaginationFilter
  {
    public List<TableFilter> Filters { get; set; } = [];
  }
}
