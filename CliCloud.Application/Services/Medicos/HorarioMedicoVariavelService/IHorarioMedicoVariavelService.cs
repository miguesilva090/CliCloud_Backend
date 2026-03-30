using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService
{
    public interface IHorarioMedicoVariavelService : ITransientService
    {
        Task<Response<IEnumerable<HorarioMedicoVariavelDTO>>> GetByMedicoIdAsync(Guid medicoId);
        Task<Response<HorarioMedicoVariavelDTO>> GetAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateHorarioMedicoVariavelRequest request);
        Task<Response<Guid>> UpdateAsync(UpdateHorarioMedicoVariavelRequest request, Guid id);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
