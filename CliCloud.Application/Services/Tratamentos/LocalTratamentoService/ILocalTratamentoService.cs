using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Filters;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService
{
    public interface ILocalTratamentoService : ITransientService
    {
        Task<Response<IEnumerable<LocalTratamentoDTO>>> GetLocalTratamentoAsync(string keyword = "");
        Task<Response<IEnumerable<LocalTratamentoLightDTO>>> GetLocalTratamentoLightAsync(string keyword = "");
        Task<PaginatedResponse<LocalTratamentoTableDTO>> GetLocalTratamentoPaginatedAsync(LocalTratamentoTableFilter filter);
        Task<Response<IEnumerable<LocalTratamentoTableDTO>>> GetAllLocalTratamentoAsync(LocalTratamentoAllFilter filter);
        Task<Response<LocalTratamentoDTO>> GetLocalTratamentoAsync(Guid id);
        Task<Response<Guid>> CreateLocalTratamentoAsync(CreateLocalTratamentoRequest request);
        Task<Response<Guid>> UpdateLocalTratamentoAsync(UpdateLocalTratamentoRequest request, Guid id);
        Task<Response<Guid>> DeleteLocalTratamentoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleLocalTratamentoAsync(IEnumerable<Guid> ids);
    }
}
