using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.Filters
{
    public class DocumentosFichaClinicaTableFilter : PaginationFilter
    {
        public Guid? UtenteId { get; set; }
        public string? Categoria { get; set; }
        public string? Tipo { get; set; }
    }
}