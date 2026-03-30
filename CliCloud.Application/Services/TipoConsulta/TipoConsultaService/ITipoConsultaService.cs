using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.DTOs;
using CliCloud.Application.Services.TiposConsulta.TipoConsultaService.Filters;

namespace CliCloud.Application.Services.TiposConsulta.TipoConsultaService
{
    public interface ITipoConsultaService : ITransientService
    {
        Task<PaginatedResponse<TipoConsultaTableDTO>> GetTipoConsultaPaginatedAsync(TipoConsultaTableFilter filter);
        Task<Response<IEnumerable<TipoConsultaTableDTO>>> GetAllTipoConsultaAsync(TipoConsultaAllFilter? filter);
        Task<Response<TipoConsultaDTO>> GetTipoConsultaAsync(Guid id);
        Task<Response<Guid>> UpdateTipoConsultaAsync(UpdateTipoConsultaRequest request, Guid id);
    }
}
