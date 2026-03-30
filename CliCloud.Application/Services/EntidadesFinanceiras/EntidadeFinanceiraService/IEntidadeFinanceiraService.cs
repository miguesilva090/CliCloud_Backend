using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Filters;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService
{
    public interface IEntidadeFinanceiraService : ITransientService
    {
        Task<Response<IEnumerable<EntidadeFinanceiraDTO>>> GetEntidadeFinanceiraAsync(string keyword = "");
        Task<Response<IEnumerable<EntidadeFinanceiraLightDTO>>> GetEntidadeFinanceiraLightAsync(string keyword = "");
        Task<PaginatedResponse<EntidadeFinanceiraTableDTO>> GetEntidadeFinanceiraPaginatedAsync(EntidadeFinanceiraTableFilter filter);
        Task<Response<IEnumerable<EntidadeFinanceiraTableDTO>>> GetAllEntidadeFinanceiraAsync(EntidadeFinanceiraAllFilter filter);
        Task<Response<EntidadeFinanceiraDTO>> GetEntidadeFinanceiraAsync(Guid id);
        Task<Response<EntidadeFinanceiraDTO>> GetEntidadeFinanceiraByNContribAsync(string ncontrib);
        Task<Response<IEnumerable<EntidadeFinanceiraDTO>>> GetEntidadeFinanceiraByNameAsync(string nome);
        Task<Response<Guid>> CreateEntidadeFinanceiraAsync(CreateEntidadeFinanceiraRequest request);
        Task<Response<Guid>> UpdateEntidadeFinanceiraAsync(UpdateEntidadeFinanceiraRequest request, Guid id);
        Task<Response<Guid>> DeleteEntidadeFinanceiraAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleEntidadeFinanceiraAsync(IEnumerable<Guid> ids);
    }
}
