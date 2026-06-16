using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;
using CliCloud.Application.Services.Pagamentos.ModoPagamentoService.Filters;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService;

public interface IModoPagamentoService : ITransientService
{
    Task<Response<IEnumerable<ModoPagamentoDTO>>> GetAsync(string keyword = "");
    Task<Response<IEnumerable<ModoPagamentoLightDTO>>> GetLightAsync(string keyword = "", bool apenasAtivos = false);
    Task<PaginatedResponse<ModoPagamentoTableDTO>> GetPaginatedAsync(ModoPagamentoTableFilter filter);
    Task<Response<IEnumerable<ModoPagamentoTableDTO>>> GetAllAsync(ModoPagamentoAllFilter? filter);
    Task<Response<ModoPagamentoDTO>> GetAsync(Guid id);
    Task<Response<Guid>> CreateAsync(CreateModoPagamentoRequest request);
    Task<Response<Guid>> UpdateAsync(UpdateModoPagamentoRequest request, Guid id);
    Task<Response<Guid>> DeleteAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
    Task<Response<Guid>> PassarHistoricoAsync(Guid id);
    Task<Response<Guid>> RetirarHistoricoAsync(Guid id);
}
