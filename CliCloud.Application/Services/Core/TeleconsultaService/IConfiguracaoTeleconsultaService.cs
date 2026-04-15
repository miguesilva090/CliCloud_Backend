using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.TeleconsultaService.DTOs;

namespace CliCloud.Application.Services.Core.TeleconsultaService
{
  public interface IConfiguracaoTeleconsultaService : ITransientService
  {
    Task<Response<ConfiguracaoTeleconsultaDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoTeleconsultaRequest request);
  }
}
