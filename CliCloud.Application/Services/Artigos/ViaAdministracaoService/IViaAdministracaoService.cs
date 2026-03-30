using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.Filters;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService
{
    public interface IViaAdministracaoService : ITransientService
    {
        Task<Response<IEnumerable<ViaAdministracaoDTO>>> GetViaAdministracaoAsync(string keyword = "");
        Task<PaginatedResponse<ViaAdministracaoTableDTO>> GetViaAdministracaoPaginatedAsync(ViaAdministracaoTableFilter filter);
        Task<Response<ViaAdministracaoDTO>> GetViaAdministracaoAsync(Guid id);
        Task<Response<Guid>> CreateViaAdministracaoAsync(CreateViaAdministracaoRequest request);
        Task<Response<Guid>> UpdateViaAdministracaoAsync(UpdateViaAdministracaoRequest request, Guid id);
        Task<Response<Guid>> DeleteViaAdministracaoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleViaAdministracaoAsync(IEnumerable<Guid> ids);
    }
}
