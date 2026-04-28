using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService
{
    public interface IHistoriaDentariaService : ITransientService
    {
        Task<Response<IReadOnlyList<HistoriaDentariaDTO>>> GetByUtenteAsync(Guid utenteId);

        Task<Response<Guid>> CreateAsync(CreateHistoriaDentariaRequest request, Guid utilizadorLogadoId);
    }
}
