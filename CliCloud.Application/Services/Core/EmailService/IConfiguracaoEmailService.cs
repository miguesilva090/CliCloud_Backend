using CliCloud.Application.Common;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.EmailService.DTOs;
using CliCloud.Application.Services.Core.EmailService.Filters;

namespace CliCloud.Application.Services.Core.EmailService;

public interface IConfiguracaoEmailService : ITransientService
{
    Task<Response<ConfiguracaoEmailDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoEmailRequest request);
    Task<Response<IEnumerable<ConfiguracaoEmailAutomaticaDTO>>> ObterConfiguracoesAutomaticasAsync(Guid clinicaId);
    Task<Response<ConfiguracaoEmailAutomaticaDTO>> ObterConfiguracaoAutomaticaAsync(Guid clinicaId, string codigo);
    Task<Response<Guid>> GuardarConfiguracaoAutomaticaAsync(Guid clinicaId, AtualizarConfiguracaoEmailAutomaticaRequest request);
    Task<PaginatedResponse<HistoricoEmailTabelaDTO>> ObterHistoricoPaginadoAsync(Guid clinicaId, HistoricoEmailTabelaFiltro filtro);
    Task<Response<Guid>> EnviarEmailPorCodigoAsync(Guid clinicaId, EnviarEmailPorCodigoRequest request);
    Task<Response<TemplatesFluxoEmailDTO>> ObterTemplatesFluxoEmailAsync(Guid clinicaId);
    Task<Response<bool>> GuardarTemplatesFluxoEmailAsync(Guid clinicaId, AtualizarTemplatesFluxoEmailRequest request);


}