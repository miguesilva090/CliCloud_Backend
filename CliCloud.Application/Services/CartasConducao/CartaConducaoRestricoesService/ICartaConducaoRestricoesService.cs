using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Filters;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService
{
  public interface ICartaConducaoRestricoesService : ITransientService
  {
    Task<Response<IEnumerable<CartaConducaoRestricoesDTO>>> GetCartaConducaoRestricoesAsync(string keyword = "");
    Task<Response<IEnumerable<CartaConducaoRestricoesLightDTO>>> GetCartaConducaoRestricoesLightAsync(string keyword = "");
    Task<PaginatedResponse<CartaConducaoRestricoesTableDTO>> GetCartaConducaoRestricoesPaginatedAsync(CartaConducaoRestricoesTableFilter filter);
    Task<Response<IEnumerable<CartaConducaoRestricoesTableDTO>>> GetAllCartaConducaoRestricoesAsync(CartaConducaoRestricoesAllFilter filter);
    Task<Response<CartaConducaoRestricoesDTO>> GetCartaConducaoRestricoesAsync(Guid id);
    Task<Response<Guid>> CreateCartaConducaoRestricoesAsync(CreateCartaConducaoRestricoesRequest request);
    Task<Response<Guid>> UpdateCartaConducaoRestricoesAsync(UpdateCartaConducaoRestricoesRequest request, Guid id);
    Task<Response<Guid>> DeleteCartaConducaoRestricoesAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleCartaConducaoRestricoesAsync(IEnumerable<Guid> ids);
  }
}
