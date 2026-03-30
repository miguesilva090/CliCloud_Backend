using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs;

namespace CliCloud.Application.Services.Tecnicos.FolgasTecnicoService
{
    public interface IFolgasTecnicoService : ITransientService
    {
        Task<Response<IEnumerable<FolgasTecnicoDTO>>> GetByTecnicoIdAsync(Guid tecnicoId);
        Task<Response<FolgasTecnicoDTO>> GetAsync(Guid id);
        Task<Response<Guid>> CreateAsync(CreateFolgasTecnicoRequest request);
        Task<Response<Guid>> UpdateAsync(UpdateFolgasTecnicoRequest request, Guid id);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);

    }
}
