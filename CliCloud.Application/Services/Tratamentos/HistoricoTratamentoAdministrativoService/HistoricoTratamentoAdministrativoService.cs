using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;

public class HistoricoTratamentoAdministrativoService(IRepositoryAsync repository)
  : IHistoricoTratamentoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;

  public async Task<PaginatedResponse<HistoricoTratamentoTableDTO>> GetPaginatedAsync(
    HistoricoTratamentoTableFilter filter
  )
  {
    filter.Modo = HistoricoTratamentoAdministrativoModos.Normalize(filter.Modo);

    // Não resetar PageNumber quando Filters tem datas/credencial — são critérios persistentes
    // da listagem (paginação quebraria na 2.ª página).

    string order =
      filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;

    var spec = new HistoricoTratamentoSearchTable(filter, order);

    return await _repository.GetPaginatedResultsAsync<
      Tratamento,
      HistoricoTratamentoTableDTO,
      Guid
    >(filter.PageNumber, filter.PageSize, spec);
  }
}
