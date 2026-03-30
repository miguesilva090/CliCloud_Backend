using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs;
using CliCloud.Application.Services.EstadosCivis.EstadoCivilService.Filters;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService
{
    public interface IEstadoCivilService : ITransientService
    {
        Task<Response<IEnumerable<EstadoCivilDTO>>> GetEstadoCivilAsync(string keyword = "");
        Task<Response<IEnumerable<EstadoCivilLightDTO>>> GetEstadoCivilLightAsync(string keyword = "");
        Task<PaginatedResponse<EstadoCivilTableDTO>> GetEstadoCivilPaginatedAsync(EstadoCivilTableFilter filter);
        Task<Response<IEnumerable<EstadoCivilTableDTO>>> GetAllEstadoCivilAsync(EstadoCivilAllFilter filter);
        Task<Response<EstadoCivilDTO>> GetEstadoCivilAsync(Guid id);
        Task<Response<Guid>> CreateEstadoCivilAsync(CreateEstadoCivilRequest request);
        Task<Response<Guid>> UpdateEstadoCivilAsync(UpdateEstadoCivilRequest request, Guid id);
        Task<Response<Guid>> DeleteEstadoCivilAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEstadoCivilAsync(IEnumerable<Guid> ids);
    }
}
