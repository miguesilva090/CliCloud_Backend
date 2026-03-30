using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService
{
    public interface ITensaoArterialService : ITransientService
    {
        Task<Response<IEnumerable<TensaoArterialDTO>>> GetTensaoArterialAsync(string keyword = "");
        Task<Response<IEnumerable<TensaoArterialLightDTO>>> GetTensaoArterialLightAsync(string keyword = "");
        Task<PaginatedResponse<TensaoArterialDTO>> GetTensaoArterialPaginatedAsync(TensaoArterialTableFilter filter);
        Task<Response<IEnumerable<TensaoArterialTableDTO>>> GetAllTensaoArterialAsync(TensaoArterialAllFilter filter);
        Task<Response<TensaoArterialDTO>> GetTensaoArterialAsync(Guid id);
        Task<Response<Guid>> CreateTensaoArterialAsync(CreateTensaoArterialRequest request);
        Task<Response<Guid>> UpdateTensaoArterialAsync(UpdateTensaoArterialRequest request, Guid id);
        Task<Response<Guid>> DeleteTensaoArterialAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTensaoArterialAsync(IEnumerable<Guid> ids);
    }
}
