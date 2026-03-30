using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.Filters
{
    public class FichaClinicaSecaoTemplateTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
    }
}
