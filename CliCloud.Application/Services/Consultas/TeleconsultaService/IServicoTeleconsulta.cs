using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService
{
  public interface IServicoTeleconsulta
  {
    Task<Response<TeleconsultaSessaoDTO>> CriarOuObterSessaoAsync(Guid clinicaId, CriarTeleconsultaRequest request);
    Task<Response<TeleconsultaSessaoDTO>> ObterPorMarcacaoAsync(Guid clinicaId, Guid consultaMarcacaoId);
    Task<Response<TeleconsultaJoinDTO>> ObterLinkEntradaAsync(Guid clinicaId, Guid sessaoId, EntrarTeleconsultaRequest request);
    Task<Response<TeleconsultaJoinDTO>> GerarLinkUtenteAsync(Guid clinicaId, Guid sessaoId, GerarLinkUtenteRequest request);
    Task<Response<Guid>> RevogarLinksSessaoAsync(Guid clinicaId, Guid sessaoId, string motivo = "Revogação manual");
    Task<Response<Guid>> TerminarSessaoAsync(Guid clinicaId, Guid sessaoId);
  }
}
