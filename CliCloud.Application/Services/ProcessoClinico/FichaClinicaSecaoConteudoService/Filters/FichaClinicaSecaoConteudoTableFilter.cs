using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Filters
{
    public class FichaClinicaSecaoConteudoTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
