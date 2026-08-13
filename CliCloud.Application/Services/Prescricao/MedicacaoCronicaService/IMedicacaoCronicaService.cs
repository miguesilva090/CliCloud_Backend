using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.DTOs;

namespace CliCloud.Application.Services.Prescricao.MedicacaoCronicaService
{
    public interface IMedicacaoCronicaService : ITransientService
    {
        Task<Response<IEnumerable<MedicacaoCronicaDTO>>> GetByUtenteIdAsync(Guid utenteId, bool apenasAtivos = true);
        Task<Response<Guid>> CreateAsync(CreateMedicacaoCronicaRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
    }
}