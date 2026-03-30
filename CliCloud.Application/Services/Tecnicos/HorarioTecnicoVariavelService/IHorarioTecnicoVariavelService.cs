using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService
{
    public interface IHorarioTecnicoVariavelService : ITransientService
    {
        Task<Response<IEnumerable<HorarioTecnicoVariavelDTO>>> GetByTecnicoIdAsync(Guid tecnicoId);
        Task<Response<HorarioTecnicoVariavelDTO>> GetAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateHorarioTecnicoVariavelRequest request);
        Task<Response<Guid>> UpdateAsync(UpdateHorarioTecnicoVariavelRequest request, Guid id);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
