using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.SmsService.DTOs;
using CliCloud.Application.Services.Core.SmsService.Filters;

namespace CliCloud.Application.Services.Core.SmsService
{
    public interface IServicoSms : ITransientService
    {
    Task<Response<ConfiguracaoSmsDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoSmsRequest request);
    
    Task<Response<IEnumerable<ConfiguracaoSmsAutomaticaDTO>>> ObterConfiguracoesAutomaticasAsync(Guid clinicaId);
    Task<Response<ConfiguracaoSmsAutomaticaDTO>> ObterConfiguracaoAutomaticaAsync(Guid clinicaId, string codigo);
    Task<Response<Guid>> GuardarConfiguracaoAutomaticaAsync(Guid clinicaId, AtualizarConfiguracaoAutomaticaRequest request);
    
    Task<Response<IEnumerable<string>>> ObterMedicosSelecionadosAsync(Guid clinicaId, string codigoConfiguracao);
    Task<Response<bool>> GuardarMedicosSelecionadosAsync(Guid clinicaId, GuardarMedicosSmsRequest request);
    Task<Response<bool>> GuardarTodosMedicosAsync(Guid clinicaId, GuardarTodosMedicosSmsRequest request);
    
    Task<PaginatedResponse<HistoricoSmsTabelaDTO>> ObterHistoricoPaginadoAsync(Guid clinicaId, HistoricoSmsTabelaFiltro filtro);
    Task<Response<Guid>> EnviarSmsTesteAsync(Guid clinicaId, EnviarSmsTesteRequest request);

    Task<Response<Guid>> EnviarSmsPorCodigoAsync(Guid clinicaId, EnviarSmsPorCodigoRequest request);
  }
}
