using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Filters;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService
{
    public interface IEstadoListaEsperaService : ITransientService
    {
        Task<Response<IEnumerable<EstadoListaEsperaDTO>>> GetEstadoListaEsperaAsync(string keyword = "");
        Task<Response<IEnumerable<EstadoListaEsperaLightDTO>>> GetEstadoListaEsperaLightAsync(string keyword = "");
        Task<PaginatedResponse<EstadoListaEsperaTableDTO>> GetEstadoListaEsperaPaginatedAsync(EstadoListaEsperaTableFilter filter);
        Task<Response<IEnumerable<EstadoListaEsperaTableDTO>>> GetAllEstadoListaEsperaAsync(EstadoListaEsperaAllFilter filter);
        Task<Response<EstadoListaEsperaDTO>> GetEstadoListaEsperaAsync(Guid id);
        Task<Response<Guid>> CreateEstadoListaEsperaAsync(CreateEstadoListaEsperaRequest request);
        Task<Response<Guid>> UpdateEstadoListaEsperaAsync(UpdateEstadoListaEsperaRequest request, Guid id);
        Task<Response<Guid>> DeleteEstadoListaEsperaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEstadoListaEsperaAsync(IEnumerable<Guid> ids);
    }
}
