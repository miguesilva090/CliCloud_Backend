using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseFuncionalService.Filters
{
    public class AnamneseOrtodonticaAnaliseFuncionalTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
