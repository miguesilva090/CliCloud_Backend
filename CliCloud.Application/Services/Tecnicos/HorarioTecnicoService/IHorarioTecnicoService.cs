using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Filters;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService
{
    public interface IHorarioTecnicoService : ITransientService
    {
        Task<Response<IEnumerable<HorarioTecnicoDTO>>> GetHorarioTecnicoAsync(string keyword = "");
        Task<Response<IEnumerable<HorarioTecnicoLightDTO>>> GetHorarioTecnicoLightAsync(string keyword = "");
        Task<PaginatedResponse<HorarioTecnicoTableDTO>> GetHorarioTecnicoPaginatedAsync(HorarioTecnicoTableFilter filter);
        Task<Response<IEnumerable<HorarioTecnicoTableDTO>>> GetAllHorarioTecnicoAsync(HorarioTecnicoAllFilter filter);
        Task<Response<HorarioTecnicoDTO>> GetHorarioTecnicoAsync(Guid id);
        Task<Response<IEnumerable<HorarioTecnicoDTO>>> GetHorarioTecnicoByTecnicoIdAsync(Guid tecnicoId);
        Task<Response<Guid>> CreateHorarioTecnicoAsync(CreateHorarioTecnicoRequest request);
        Task<Response<Guid>> UpdateHorarioTecnicoAsync(UpdateHorarioTecnicoRequest request, Guid id);
        Task<Response<Guid>> DeleteHorarioTecnicoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleHorarioTecnicoAsync(IEnumerable<Guid> ids);
    }
}
