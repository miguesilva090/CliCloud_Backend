using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Filters
{
    public class AntecedentesPessoaisTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public AntecedentesPessoaisTableFilter()
        {
            Filters = [];
        }
    }
}
