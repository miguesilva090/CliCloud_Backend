using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs
{
    public class DeleteMultipleDocumentosFichaClinicaRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}