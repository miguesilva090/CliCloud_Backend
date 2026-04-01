using System;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ChamadaUtentesService.DTOs;

namespace CliCloud.Application.Services.Core.ChamadaUtentesService
{
  public interface IChamadaUtentesService : ITransientService
  {
    Task<Response<ChamadaUtenteDadosDTO>> ObterDadosChamadaConsultaAsync(Guid clinicaId, Guid marcacaoConsultaId, bool chamarOutraVez);
    Task<Response<ChamadaUtenteDadosDTO>> ObterDadosChamadaTratamentoAsync(Guid clinicaId, Guid sessaoTratamentoId, bool chamarOutraVez);

    Task<Response<Guid>> ChamarUtenteConsultaAsync(Guid clinicaId, Guid marcacaoConsultaId, ChamarConsultaRequest request);
    Task<Response<Guid>> ChamarUtenteTratamentoAsync(Guid clinicaId, Guid sessaoTratamentoId, ChamarSessaoTratamentoRequest request);

    Task<Response<bool>> AtualizarEstadoAsync(Guid clinicaId, Guid chamadaId, int estado);
    Task<Response<IEnumerable<ChamadaUtenteFilaItemDTO>>> ObterChamadasAsync(Guid clinicaId, string? tipo = null);
  }
}
