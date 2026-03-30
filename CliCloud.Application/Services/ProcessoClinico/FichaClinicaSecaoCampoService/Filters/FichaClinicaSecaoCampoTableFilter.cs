using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoCampoService.Filters
{
    public class FichaClinicaSecaoCampoTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }

        public Guid? SeparadorId { get; set; }
    }
}

