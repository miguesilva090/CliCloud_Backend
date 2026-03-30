using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Filters
{
    public class AnamneseOdontopediatriaTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        public Guid? UtenteId { get; set; }
    }
}
