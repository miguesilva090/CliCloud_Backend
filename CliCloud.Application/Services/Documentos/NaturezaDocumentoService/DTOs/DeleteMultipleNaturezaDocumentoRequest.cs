using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs
{
    public class DeleteMultipleNaturezaDocumentoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = [];
    }
}