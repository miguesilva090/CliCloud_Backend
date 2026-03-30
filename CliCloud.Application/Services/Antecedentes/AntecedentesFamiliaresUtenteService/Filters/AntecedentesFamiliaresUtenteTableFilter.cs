using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Filters
{
    public class AntecedentesFamiliaresUtenteTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public AntecedentesFamiliaresUtenteTableFilter()
        {
            Filters = [];
        }
    }
}
