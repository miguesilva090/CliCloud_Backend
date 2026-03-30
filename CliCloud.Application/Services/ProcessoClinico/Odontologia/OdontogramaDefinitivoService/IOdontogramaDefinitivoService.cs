using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService
{
    public interface IOdontogramaDefinitivoService : ITransientService
    {
        Task<Response<IEnumerable<OdontogramaDefinitivoDTO>>> GetOdontogramaDefinitivoAsync(string keyword = "");
        Task<PaginatedResponse<OdontogramaDefinitivoDTO>> GetOdontogramaDefinitivoPaginatedAsync(OdontogramaDefinitivoTableFilter filter);
        Task<Response<OdontogramaDefinitivoDTO>> GetOdontogramaDefinitivoAsync(Guid id);
        Task<Response<IEnumerable<OdontogramaDefinitivoDTO>>> GetByUtenteConsultaAsync(Guid utenteId, Guid consultaId);
        Task<Response<Guid>> CreateOdontogramaDefinitivoAsync(CreateOdontogramaDefinitivoRequest request);
        Task<Response<Guid>> UpdateOdontogramaDefinitivoAsync(UpdateOdontogramaDefinitivoRequest request, Guid id);
        Task<Response<Guid>> DeleteOdontogramaDefinitivoAsync(Guid id);
    }
}

