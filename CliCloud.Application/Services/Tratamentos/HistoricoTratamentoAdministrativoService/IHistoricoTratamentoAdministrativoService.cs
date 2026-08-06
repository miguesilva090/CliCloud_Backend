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
}