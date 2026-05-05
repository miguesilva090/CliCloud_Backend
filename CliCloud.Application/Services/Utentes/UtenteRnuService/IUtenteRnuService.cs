using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utentes.UtenteRnuService.DTOs;

namespace CliCloud.Application.Services.Utentes.UtenteRnuService;

public interface IUtenteRnuService : ITransientService
{
    Task<Response<ConsultarUtenteRnuResponse>> ConsultarUtenteAsync(Guid clinicaId, ConsultarUtenteRnuRequest request);
}