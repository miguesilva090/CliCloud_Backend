using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService;

public interface IConfigWebServiceService : ITransientService
{
    Task<Response<ConfigWebServiceDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigWebServiceRequest request);
}