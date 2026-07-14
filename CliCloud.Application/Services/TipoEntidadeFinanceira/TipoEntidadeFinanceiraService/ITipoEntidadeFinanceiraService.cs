using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Filters;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService
{
    public interface ITipoEntidadeFinanceiraService : ITransientService
    {
        Task<Response<IEnumerable<TipoEntidadeFinanceiraDTO>>> GetTipoEntidadeFinanceiraAsync(string keyword = "");
        Task<Response<IEnumerable<TipoEntidadeFinanceiraLightDTO>>> GetTipoEntidadeFinanceiraLightAsync(string keyword = "");
        Task<PaginatedResponse<TipoEntidadeFinanceiraTableDTO>> GetTipoEntidadeFinanceiraPaginatedAsync(TipoEntidadeFinanceiraTableFilter filter);
        Task<Response<IEnumerable<TipoEntidadeFinanceiraTableDTO>>> GetAllTipoEntidadeFinanceiraAsync(TipoEntidadeFinanceiraAllFilter filter);
        Task<Response<TipoEntidadeFinanceiraDTO>> GetTipoEntidadeFinanceiraAsync(Guid id);
        Task<Response<TipoEntidadeFinanceiraDTO>> GetTipoEntidadeFinanceiraBySiglaAsync(string sigla);
        Task<Response<Guid>> CreateTipoEntidadeFinanceiraAsync(CreateTipoEntidadeFinanceiraRequest request);
        Task<Response<Guid>> UpdateTipoEntidadeFinanceiraAsync(UpdateTipoEntidadeFinanceiraRequest request, Guid id);
        Task<Response<Guid>> DeleteTipoEntidadeFinanceiraAsync(Guid id);
        Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoEntidadeFinanceiraAsync(IEnumerable<Guid> ids);
    }
}
