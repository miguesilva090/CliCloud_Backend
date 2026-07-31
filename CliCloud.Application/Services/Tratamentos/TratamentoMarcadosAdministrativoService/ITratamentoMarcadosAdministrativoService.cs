using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService.Filters;

namespace CliCloud.Application.Services.Tratamentos.TratamentoMarcadosAdministrativoService;

public interface ITratamentoMarcadosAdministrativoService : ITransientService
{
  Task<PaginatedResponse<TratamentoMarcadosTableDTO>> GetPaginatedAsync(
    TratamentoMarcadosTableFilter filter
  );
}
