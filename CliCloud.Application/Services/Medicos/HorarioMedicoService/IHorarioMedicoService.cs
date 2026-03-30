using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Filters;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService
{
    public interface IHorarioMedicoService : ITransientService
    {
        Task<Response<IEnumerable<HorarioMedicoDTO>>> GetHorarioMedicoAsync(string keyword = "");
        Task<Response<IEnumerable<HorarioMedicoLightDTO>>> GetHorarioMedicoLightAsync(string keyword = "");
        Task<PaginatedResponse<HorarioMedicoTableDTO>> GetHorarioMedicoPaginatedAsync(HorarioMedicoTableFilter filter);
        Task<Response<IEnumerable<HorarioMedicoTableDTO>>> GetAllHorarioMedicoAsync(HorarioMedicoAllFilter filter);
        Task<Response<HorarioMedicoDTO>> GetHorarioMedicoAsync(Guid id);
        Task<Response<IEnumerable<HorarioMedicoDTO>>> GetHorarioMedicoByMedicoIdAsync(Guid medicoId);
        Task<Response<Guid>> CreateHorarioMedicoAsync(CreateHorarioMedicoRequest request);
        Task<Response<Guid>> UpdateHorarioMedicoAsync(UpdateHorarioMedicoRequest request, Guid id);
        Task<Response<Guid>> DeleteHorarioMedicoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioMedicoAsync(IEnumerable<Guid> ids);
    }
}
