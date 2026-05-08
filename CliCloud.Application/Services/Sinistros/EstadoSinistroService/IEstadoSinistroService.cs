using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs;
using CliCloud.Application.Services.Sinistros.EstadoSinistroService.Filters;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService
{
    public interface IEstadoSinistroService : ITransientService
    {
        Task<PaginatedResponse<EstadoSinistroDTO>> GetPaginatedAsync(EstadoSinistroTableFilter filter);
        Task<Response<EstadoSinistroDTO>> GetByIdAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateEstadoSinistroRequest request);
        Task<Response<Guid>> UpdateAsync(Guid id, UpdateEstadoSinistroRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
    }
}