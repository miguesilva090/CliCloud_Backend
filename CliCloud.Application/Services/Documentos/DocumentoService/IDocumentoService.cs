using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.DocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoService.Filters;

namespace CliCloud.Application.Services.Documentos.DocumentoService
{
    public interface IDocumentoService : ITransientService
    {
        Task<Response<IEnumerable<DocumentoDTO>>> GetDocumentoAsync(string keyword = "");
        Task<Response<IEnumerable<DocumentoLightDTO>>> GetDocumentoLightAsync(string keyword = "");
        Task<PaginatedResponse<DocumentoTableDTO>> GetDocumentoPaginatedAsync(DocumentoTableFilter filter);
        Task<Response<IEnumerable<DocumentoTableDTO>>> GetAllDocumentoAsync(DocumentoAllFilter filter);
        Task<Response<DocumentoDTO>> GetDocumentoAsync(Guid id);
        Task<Response<DocumentoDTO>> GetDocumentoByTipoNumeroAsync(Guid tipoDocumentoId, int numeroDocumento);
        Task<Response<Guid>> CreateDocumentoAsync(CreateDocumentoRequest request);
        Task<Response<Guid>> UpdateDocumentoAsync(UpdateDocumentoRequest request, Guid id);
        Task<Response<Guid>> DeleteDocumentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleDocumentoAsync(IEnumerable<Guid> ids);
    }
}
