using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Filters;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService
{
    public interface ITipoDocumentoService : ITransientService
    {
        Task<Response<IEnumerable<TipoDocumentoDTO>>> GetTipoDocumentoAsync(string keyword = "");
        Task<Response<IEnumerable<TipoDocumentoLightDTO>>> GetTipoDocumentoLightAsync(string keyword = "");
        Task<PaginatedResponse<TipoDocumentoTableDTO>> GetTipoDocumentoPaginatedAsync(TipoDocumentoTableFilter filter);
        Task<Response<IEnumerable<TipoDocumentoTableDTO>>> GetAllTipoDocumentoAsync(TipoDocumentoAllFilter filter);
        Task<Response<TipoDocumentoDTO>> GetTipoDocumentoAsync(Guid id);
        Task<Response<TipoDocumentoDTO>> GetTipoDocumentoByAbreviaturaAsync(string abreviatura);
        Task<Response<Guid>> CreateTipoDocumentoAsync(CreateTipoDocumentoRequest request);
        Task<Response<Guid>> UpdateTipoDocumentoAsync(UpdateTipoDocumentoRequest request, Guid id);
        Task<Response<Guid>> DeleteTipoDocumentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoDocumentoAsync(IEnumerable<Guid> ids);
    }
}
