using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.DTOs;
using CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService.Filters;

namespace CliCloud.Application.Services.Faturacao.ConfiguracaoADSEService
{
  public interface IConfiguracaoADSEService : ITransientService
  {
    Task<Response<ConfiguracaoADSEDTO>> ObterConfiguracaoPorEmpresaAsync(Guid empresaId);
    Task<Response<Guid>> GuardarConfiguracaoAsync(Guid empresaId, GuardarConfiguracaoADSERequest request);
    Task<Response<IEnumerable<ConfiguracaoADSEDTO>>> GetConfiguracaoADSEAsync(string keyword = "");
    Task<PaginatedResponse<ConfiguracaoADSEDTO>> GetConfiguracaoADSEPaginatedAsync(ConfiguracaoADSETableFilter filter);
    Task<Response<ConfiguracaoADSEDTO>> GetConfiguracaoADSEAsync(Guid id);
    Task<Response<Guid>> CreateConfiguracaoADSEAsync(CreateConfiguracaoADSERequest request);
    Task<Response<Guid>> UpdateConfiguracaoADSEAsync(UpdateConfiguracaoADSERequest request, Guid id);
    Task<Response<Guid>> DeleteConfiguracaoADSEAsync(Guid id);
  }
}
