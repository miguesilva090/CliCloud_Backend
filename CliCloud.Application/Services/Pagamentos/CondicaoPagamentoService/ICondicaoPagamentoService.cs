using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.Filters;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService;

public interface ICondicaoPagamentoService : ITransientService
{
    Task<Response<IEnumerable<CondicaoPagamentoDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<CondicaoPagamentoLightDTO>>> GetLightAsync(string keyword = "");
    Task<PaginatedResponse<CondicaoPagamentoTableDTO>> GetPaginatedAsync(CondicaoPagamentoTableFilter filter);
    Task<Response<IEnumerable<CondicaoPagamentoTableDTO>>> GetAllAsync(CondicaoPagamentoAllFilter? filter);
    Task<Response<CondicaoPagamentoDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateCondicaoPagamentoRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateCondicaoPagamentoRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
}
