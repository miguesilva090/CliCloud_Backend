using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService
{
    public interface IUtentePatologiaComparticipacaoService : ITransientService
    {
        Task<Response<IEnumerable<UtentePatologiaComparticipacaoDTO>>> GetByUtenteIdAsync(Guid utenteId);
        Task<Response<Guid>> CreateAsync(CreateUtentePatologiaComparticipacaoRequest request);
        Task<Response<Guid>> DeleteAsync(Guid id);
    }
}