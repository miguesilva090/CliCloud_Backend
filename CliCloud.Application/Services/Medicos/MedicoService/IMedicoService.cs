using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MedicoService.Filters;

namespace CliCloud.Application.Services.Medicos.MedicoService
{
    public interface IMedicoService : ITransientService
    {
        Task<Response<IEnumerable<MedicoDTO>>> GetMedicoAsync(string keyword = "");
        Task<Response<IEnumerable<MedicoLightDTO>>> GetMedicoLightAsync(string keyword = "");
        Task<PaginatedResponse<MedicoTableDTO>> GetMedicoPaginatedAsync(MedicoTableFilter filter);
        Task<Response<IEnumerable<MedicoTableDTO>>> GetAllMedicoAsync(MedicoAllFilter filter);
        Task<Response<MedicoDTO>> GetMedicoAsync(Guid id);
        Task<Response<MedicoDTO>> GetMedicoByNContribAsync(string ncontrib);
        Task<Response<MedicoDTO?>> GetMedicoByIdUtilizadorAsync(Guid idUtilizador);
        Task<Response<IEnumerable<MedicoDTO>>> GetMedicoByNameAsync(string nome);
        Task<Response<Guid>> CreateMedicoAsync(CreateMedicoRequest request);
        Task<Response<Guid>> UpdateMedicoAsync(UpdateMedicoRequest request, Guid id);
        Task<Response<Guid>> DeleteMedicoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMedicoAsync(IEnumerable<Guid> ids);
    }
}
