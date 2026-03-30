using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Filters;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService
{
    public interface IHorarioTecnicoDiaService : ITransientService
    {
        Task<Response<IEnumerable<HorarioTecnicoDiaDTO>>> GetHorarioTecnicoDiaAsync(string keyword = "");
        Task<Response<IEnumerable<HorarioTecnicoDiaLightDTO>>> GetHorarioTecnicoDiaLightAsync(string keyword = "");
        Task<PaginatedResponse<HorarioTecnicoDiaTableDTO>> GetHorarioTecnicoDiaPaginatedAsync(HorarioTecnicoDiaTableFilter filter);
        Task<Response<IEnumerable<HorarioTecnicoDiaTableDTO>>> GetAllHorarioTecnicoDiaAsync(HorarioTecnicoDiaAllFilter filter);
        Task<Response<HorarioTecnicoDiaDTO>> GetHorarioTecnicoDiaAsync(Guid id);
        Task<Response<IEnumerable<HorarioTecnicoDiaDTO>>> GetHorarioTecnicoDiaByHorarioTecnicoIdAsync(Guid horarioTecnicoId);
        Task<Response<Guid>> CreateHorarioTecnicoDiaAsync(CreateHorarioTecnicoDiaRequest request);
        Task<Response<Guid>> UpdateHorarioTecnicoDiaAsync(UpdateHorarioTecnicoDiaRequest request, Guid id);
        Task<Response<Guid>> DeleteHorarioTecnicoDiaAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioTecnicoDiaAsync(IEnumerable<Guid> ids);
    }
}
