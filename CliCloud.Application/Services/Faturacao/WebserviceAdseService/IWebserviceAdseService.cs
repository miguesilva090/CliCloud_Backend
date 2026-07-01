using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;

namespace CliCloud.Application.Services.Faturacao.WebserviceAdseService;

public interface IWebserviceAdseService : ITransientService
{
    Task<Response<WebserviceAdseDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarWebserviceAdseRequest request);
    Task<Response<IReadOnlyList<AdseOrganismoLookupDTO>>> ListarOrganismosAsync();
}