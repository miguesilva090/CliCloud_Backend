using System;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ChamadaVozService.DTOs;

namespace CliCloud.Application.Services.Core.ChamadaVozService
{
  public interface IServicoChamadaVoz : ITransientService
  {
    Task<Response<ConfiguracaoChamadaVozDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<ConfiguracaoChamadaVozOpcoesDTO>> ObterOpcoesAsync();
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoChamadaVozRequest request);
  }
}
