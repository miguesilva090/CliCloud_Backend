using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Sexos.SexoService.DTOs;
using CliCloud.Application.Services.Sexos.SexoService.Filters;

namespace CliCloud.Application.Services.Sexos.SexoService
{
    public interface ISexoService : ITransientService
    {
        Task<Response<IEnumerable<SexoDTO>>> GetSexoAsync(string keyword = "");
        Task<Response<IEnumerable<SexoLightDTO>>> GetSexoLightAsync(string keyword = "");
        Task<PaginatedResponse<SexoTableDTO>> GetSexoPaginatedAsync(SexoTableFilter filter);
        Task<Response<IEnumerable<SexoTableDTO>>> GetAllSexoAsync(SexoAllFilter filter);
        Task<Response<SexoDTO>> GetSexoAsync(Guid id);
        Task<Response<Guid>> CreateSexoAsync(CreateSexoRequest request);
        Task<Response<Guid>> UpdateSexoAsync(UpdateSexoRequest request, Guid id);
        Task<Response<Guid>> DeleteSexoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleSexoAsync(IEnumerable<Guid> ids);
    }
}

