using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService.DTOs;

namespace CliCloud.Application.Services.Prescricao.MedicacaoFavoritaService
{
    public interface IMedicacaoFavoritaService : ITransientService
    {
        Task<Response<IEnumerable<MedicacaoFavoritaDTO>>> GetByMedicoIdAsync(Guid medicoId, int? tipoLinha = null);
        Task<Response<Guid>> CreateAsync(CreateMedicacaoFavoritaRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
    }
}