using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaATMService.Filters
{
    public class AnamneseOrtodonticaATMTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
