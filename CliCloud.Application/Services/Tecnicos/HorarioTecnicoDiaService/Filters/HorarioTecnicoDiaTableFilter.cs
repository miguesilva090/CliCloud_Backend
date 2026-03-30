using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Filters
{
    public class HorarioTecnicoDiaTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public HorarioTecnicoDiaTableFilter()
        {
            Filters = [];
        }
    }
}
