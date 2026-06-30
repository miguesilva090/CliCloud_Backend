using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Filters;

namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public interface ICredenciaisSnsService : ITransientService
{
    Task<PaginatedResponse<CredenciaisSnsLoteTableDTO>> GetPaginatedAsync(CredenciaisSnsTableFilter filter);
    Task<Response<bool>> DeleteAsync(string modulo, DeleteCredenciaisSnsRequest request, CancellationToken cancellationToken = default);
}
