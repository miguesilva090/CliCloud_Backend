using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Filters
{
    public class HorarioMedicoDiaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public HorarioMedicoDiaTableFilter()
        {
            Filters = [];
        }
    }
}
