using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs;
using CliCloud.Application.Services.Tratamentos.GoniometriasService.Filters;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService
{
    public interface IGoniometriasService : ITransientService
    {
        Task<Response<IEnumerable<GoniometriasDTO>>> GetGoniometriasAsync(string keyword = "");
        Task<Response<IEnumerable<GoniometriasLightDTO>>> GetGoniometriasLightAsync(string keyword = "");
        Task<Response<IEnumerable<GoniometriasTableDTO>>> GetAllGoniometriasAsync(GoniometriasAllFilter filter);
        Task<PaginatedResponse<GoniometriasTableDTO>> GetGoniometriasPaginatedAsync(GoniometriasTableFilter filter);
        Task<Response<GoniometriasDTO>> GetGoniometriasAsync(Guid id);
        Task<Response<GoniometriasDTO>> GetGoniometriasByDescricaoAsync(string descricao);
        Task<Response<Guid>> CreateGoniometriasAsync(CreateGoniometriasRequest request);
        Task<Response<Guid>> UpdateGoniometriasAsync(UpdateGoniometriasRequest request, Guid id);
        Task<Response<Guid>> DeleteGoniometriasAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleGoniometriasAsync(IEnumerable<Guid> ids);
    }
}
