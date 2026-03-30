using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Filters
{
    public class ProvenienciaUtenteTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public ProvenienciaUtenteTableFilter()
        {
            Filters = [];
        }
    }
}
