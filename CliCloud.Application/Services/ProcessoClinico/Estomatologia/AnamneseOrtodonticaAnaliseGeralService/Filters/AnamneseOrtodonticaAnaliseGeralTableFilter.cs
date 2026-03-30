using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseGeralService.Filters
{
    public class AnamneseOrtodonticaAnaliseGeralTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
