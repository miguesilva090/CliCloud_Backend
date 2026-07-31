using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsFisioterapiaGateway : ITransientService
{
    Task<PaginatedResponse<CredenciaisSnsLoteTableDTO>> GetPaginatedAsync(
        List<TableFilter> filters,
        int pageNumber,
        int pageSize,
        string? orderBy,
        int? filtroLegado,
        CancellationToken cancellationToken = default
    );
}