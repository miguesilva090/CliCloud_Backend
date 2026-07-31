using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService;

public class TratamentoMarcadosAdministrativoService(IRepositoryAsync repository)
  : ITratamentoMarcadosAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;

  public async Task<PaginatedResponse<TratamentoMarcadosTableDTO>> GetPaginatedAsync(
    TratamentoMarcadosTableFilter filter
  )
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order =
      filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new TratamentoMarcadosSearchTable(filter, order);

    return await _repository.GetPaginatedResultsAsync<
      Tratamento,
      TratamentoMarcadosTableDTO,
      Guid
    >(filter.PageNumber, filter.PageSize, spec);
  }
}
