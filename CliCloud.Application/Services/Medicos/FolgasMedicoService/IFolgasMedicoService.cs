using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs;

namespace CliCloud.Application.Services.Medicos.FolgasMedicoService
{
    public interface IFolgasMedicoService : ITransientService
    {
        Task<Response<IEnumerable<FolgasMedicoDTO>>> GetByMedicoIdAsync(Guid medicoId);
        Task<Response<FolgasMedicoDTO>> GetAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateFolgasMedicoRequest request);
        Task<Response<Guid>> UpdateAsync(UpdateFolgasMedicoRequest request, Guid id);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
