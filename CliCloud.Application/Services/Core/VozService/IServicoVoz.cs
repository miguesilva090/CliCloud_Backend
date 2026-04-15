using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.VozService.DTOs;

namespace CliCloud.Application.Services.Core.VozService
{
  public interface IServicoVoz : ITransientService
  {
    Task<Response<ConfiguracaoVozDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<ConfiguracaoVozOpcoesDTO>> ObterOpcoesAsync();
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoVozRequest request);
  }
}
