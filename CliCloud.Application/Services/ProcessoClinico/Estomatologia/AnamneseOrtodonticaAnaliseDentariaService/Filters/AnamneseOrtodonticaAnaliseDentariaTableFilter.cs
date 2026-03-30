using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOrtodonticaAnaliseDentariaService.Filters
{
    public class AnamneseOrtodonticaAnaliseDentariaTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
