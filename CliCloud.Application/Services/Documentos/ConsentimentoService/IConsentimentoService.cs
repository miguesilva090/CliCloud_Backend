using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;

namespace CliCloud.Application.Services.Documentos.ConsentimentoService;

public interface IConsentimentoService : ITransientService
{
    Task<Response<PedidoConsentimentoDTO>> CriarPedidoAsync(CriarPedidoConsentimentoRequest request);
    Task<Response<PedidoConsentimentoDTO>> ObterPedidoAsync(Guid id);
    Task<Response<IEnumerable<PedidoConsentimentoDTO>>> ObterPedidosAsync(
        Guid? utenteId = null,
        int? estado = null
    );
    Task<Response<Guid>> CancelarPedidoAsync(Guid id, string? observacoes = null);
    Task<Response<Guid>> MarcarAssinadoAsync(Guid id, MarcarPedidoConsentimentoAssinadoRequest request);
}