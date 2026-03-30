using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Filters
{
    public class TiposTratamentoDentarioTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
