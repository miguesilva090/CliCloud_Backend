using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService
{
    public interface IEstadosDentariosService : ITransientService
    {
        Task<Response<IEnumerable<EstadosDentariosDTO>>> GetEstadosDentariosAsync(string keyword = "");
        Task<PaginatedResponse<EstadosDentariosDTO>> GetEstadosDentariosPaginatedAsync(EstadosDentariosTableFilter filter);
        Task<Response<EstadosDentariosDTO>> GetEstadosDentariosAsync(Guid id);
        Task<Response<Guid>> CreateEstadosDentariosAsync(CreateEstadosDentariosRequest request);
        Task<Response<Guid>> UpdateEstadosDentariosAsync(UpdateEstadosDentariosRequest request, Guid id);
        Task<Response<Guid>> DeleteEstadosDentariosAsync(Guid id);
    }
}
