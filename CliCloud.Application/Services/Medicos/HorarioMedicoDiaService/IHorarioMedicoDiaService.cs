using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Filters;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService
{
    public interface IHorarioMedicoDiaService : ITransientService
    {
        Task<Response<IEnumerable<HorarioMedicoDiaDTO>>> GetHorarioMedicoDiaAsync(string keyword = "");
        Task<Response<IEnumerable<HorarioMedicoDiaLightDTO>>> GetHorarioMedicoDiaLightAsync(string keyword = "");
        Task<PaginatedResponse<HorarioMedicoDiaTableDTO>> GetHorarioMedicoDiaPaginatedAsync(HorarioMedicoDiaTableFilter filter);
        Task<Response<IEnumerable<HorarioMedicoDiaTableDTO>>> GetAllHorarioMedicoDiaAsync(HorarioMedicoDiaAllFilter filter);
        Task<Response<HorarioMedicoDiaDTO>> GetHorarioMedicoDiaAsync(Guid id);
        Task<Response<IEnumerable<HorarioMedicoDiaDTO>>> GetHorarioMedicoDiaByHorarioMedicoIdAsync(Guid horarioMedicoId);
        Task<Response<Guid>> CreateHorarioMedicoDiaAsync(CreateHorarioMedicoDiaRequest request);
        Task<Response<Guid>> UpdateHorarioMedicoDiaAsync(UpdateHorarioMedicoDiaRequest request, Guid id);
        Task<Response<Guid>> DeleteHorarioMedicoDiaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioMedicoDiaAsync(IEnumerable<Guid> ids);
    }
}
