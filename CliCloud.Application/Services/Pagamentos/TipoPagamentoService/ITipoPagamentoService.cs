using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Pagamentos.TipoPagamentoService.DTOs;

namespace CliCloud.Application.Services.Pagamentos.TipoPagamentoService;

public interface ITipoPagamentoService : ITransientService
{
    Task<Response<IEnumerable<TipoPagamentoLightDTO>>> GetLightAsync(string keyword = "");
}
