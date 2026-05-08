using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Sinistros.SinistradoService.DTOs;
using CliCloud.Application.Services.Sinistros.SinistradoService.Filters;

namespace CliCloud.Application.Services.Sinistros.SinistradoService
{
    public interface ISinistradoService : ITransientService
    {
        Task<PaginatedResponse<SinistradoTableDTO>> GetPaginatedAsync(SinistradoTableFilter filter);
        Task<Response<SinistradoDTO>> GetByIdAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateSinistradoRequest request);
        Task<Response<Guid>> UpdateAsync(Guid id, UpdateSinistradoRequest request);
        Task<Response<Guid>> MoveToHistoryAsync(Guid id);
        Task<Response<Guid>> RestoreFromHistoryAsync(Guid id);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<List<SinistradoLinhaServicoDTO>>> GetUnbilledServicesByUtenteIdAsync(Guid utenteId);
    }
}