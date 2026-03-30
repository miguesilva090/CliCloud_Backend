using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Filters
{
    public class HorarioTecnicoTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public HorarioTecnicoTableFilter()
        {
            Filters = [];
        }
    }
}
