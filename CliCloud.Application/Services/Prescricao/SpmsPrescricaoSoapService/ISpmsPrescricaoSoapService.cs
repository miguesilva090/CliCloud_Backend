using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;

public interface ISpmsPrescricaoSoapService : ITransientService
{
    Task<Response<ProxyTokenResultDTO>> ObterTokenCredAsync(Guid clinicaId, ObterTokenCredRequest request);
    Task<Response<ProxyTokenResultDTO>> ObterTokenCcAsync(Guid clinicaId, ObterTokenAssinadoRequest request);
    Task<Response<ProxyTokenResultDTO>> ObterTokenComAsync(Guid clinicaId, ObterTokenAssinadoRequest request);
    Task<Response<SpmsSoapOperationResultDTO>> ExecutarConsultaUtenteAsync(Guid clinicaId, ConsultaUtenteRequest request);
    Task<Response<SpmsSoapOperationResultDTO>> ExecutarRegistoPrescricaoAsync(Guid clinicaId, RegistoPrescricaoRequest request);
    Task<Response<SpmsSoapOperationResultDTO>> ExecutarRegistoPrescricaoRspAsync(Guid clinicaId, RegistoPrescricaoRspRequest request);
}