using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.MargemMedicoService.Filters;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService
{
    public interface IMargemMedicoService : ITransientService
    {
        Task<Response<IEnumerable<MargemMedicoDTO>>> GetMargemMedicoAsync(string keyword = "");
        Task<Response<IEnumerable<MargemMedicoLightDTO>>> GetMargemMedicoLightAsync(string keyword = "");
        Task<PaginatedResponse<MargemMedicoTableDTO>> GetMargemMedicoPaginatedAsync(MargemMedicoTableFilter filter);
        Task<Response<IEnumerable<MargemMedicoTableDTO>>> GetAllMargemMedicoAsync(MargemMedicoAllFilter filter);
        Task<Response<MargemMedicoDTO>> GetMargemMedicoAsync(Guid id);
        Task<Response<IEnumerable<MargemMedicoDTO>>> GetMargemMedicoByMedicoIdAsync(Guid medicoId);
        Task<Response<Guid>> CreateMargemMedicoAsync(CreateMargemMedicoRequest request);
        Task<Response<Guid>> UpdateMargemMedicoAsync(UpdateMargemMedicoRequest request, Guid id);
        Task<Response<Guid>> DeleteMargemMedicoAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleMargemMedicoAsync(IEnumerable<Guid> ids);
    }
}
