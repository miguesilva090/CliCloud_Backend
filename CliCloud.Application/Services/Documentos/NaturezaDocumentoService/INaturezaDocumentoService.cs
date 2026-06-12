using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Filters;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService
{
    public interface INaturezaDocumentoService : ITransientService
    {
        Task<Response<IEnumerable<NaturezaDocumentoDTO>>> GetNaturezaDocumentoAsync(string keyword = "");
        Task<Response<IEnumerable<NaturezaDocumentoLightDTO>>> GetNaturezaDocumentoLightAsync(string keyword = "");
        Task<PaginatedResponse<NaturezaDocumentoTableDTO>> GetNaturezaDocumentoPaginatedAsync(NaturezaDocumentoTableFilter filter);
        Task<Response<IEnumerable<NaturezaDocumentoTableDTO>>> GetAllNaturezaDocumentoAsync(NaturezaDocumentoAllFilter? filter);
        Task<Response<NaturezaDocumentoDTO>> GetNaturezaDocumentoAsync(Guid id);
        Task<Response<Guid>> CreateNaturezaDocumentoAsync(CreateNaturezaDocumentoRequest request);
        Task<Response<Guid>> UpdateNaturezaDocumentoAsync(UpdateNaturezaDocumentoRequest request, Guid id);
        Task<Response<Guid>> DeleteNaturezaDocumentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleNaturezaDocumentoAsync(IEnumerable<Guid> ids);
    }
}