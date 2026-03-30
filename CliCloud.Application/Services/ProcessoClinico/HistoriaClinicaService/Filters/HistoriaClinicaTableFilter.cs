using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Filters
{
    public class HistoriaClinicaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; } = [];
    }
}
