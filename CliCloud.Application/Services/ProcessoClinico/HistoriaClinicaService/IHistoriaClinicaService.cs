using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Filters;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService
{
    public interface IHistoriaClinicaService : ITransientService
    {
        Task<Response<IEnumerable<HistoriaClinicaDTO>>> GetHistoriaClinicaAsync(string keyword = "");
        Task<Response<IEnumerable<HistoriaClinicaLightDTO>>> GetHistoriaClinicaLightAsync(string keyword = "");
        Task<PaginatedResponse<HistoriaClinicaTableDTO>> GetHistoriaClinicaPaginatedAsync(HistoriaClinicaTableFilter filter);
        Task<Response<IEnumerable<HistoriaClinicaTableDTO>>> GetAllHistoriaClinicaAsync(HistoriaClinicaAllFilter filter);
        Task<Response<HistoriaClinicaDTO>> GetHistoriaClinicaAsync(Guid id);
        Task<Response<Guid>> CreateHistoriaClinicaAsync(CreateHistoriaClinicaRequest request);
        Task<Response<Guid>> UpdateHistoriaClinicaAsync(UpdateHistoriaClinicaRequest request, Guid id);
        Task<Response<Guid>> DeleteHistoriaClinicaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHistoriaClinicaAsync(IEnumerable<Guid> ids);
    }
}
