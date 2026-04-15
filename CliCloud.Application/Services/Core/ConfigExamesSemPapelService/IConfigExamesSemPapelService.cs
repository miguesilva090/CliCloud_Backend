using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigExamesSemPapelService.DTOs;

namespace CliCloud.Application.Services.Core.ConfigExamesSemPapelService;

public interface IConfigExamesSemPapelService : ITransientService
{
    Task<Response<ConfigExamesSemPapelDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigExamesSemPapelRequest request);
}