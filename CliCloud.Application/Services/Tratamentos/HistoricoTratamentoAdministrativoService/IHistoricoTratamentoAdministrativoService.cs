using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;

public interface IHistoricoTratamentoAdministrativoService : ITransientService
{
  Task<PaginatedResponse<HistoricoTratamentoTableDTO>> GetPaginatedAsync(
    HistoricoTratamentoTableFilter filter
  );

  Task<Response<Guid>> PassarParaHistoricoAsync(Guid id);

  Task<Response<Guid>> ReabrirAsync(Guid id);

  Task<Response<Guid>> DeleteAsync(Guid id);

  Task<Response<HistoricoTratamentoObservacoesDTO>> GetObservacoesAsync(Guid id);

  Task<Response<Guid>> AppendObservacaoAsync(
    Guid id,
    AppendHistoricoTratamentoObservacaoRequest request
  );
}
