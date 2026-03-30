using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.Filters;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService
{
  public interface ICartaConducaoService : ITransientService
  {
    Task<Response<IEnumerable<CartaConducaoDTO>>> GetCartaConducaoAsync(string keyword = "");
    Task<Response<IEnumerable<CartaConducaoLightDTO>>> GetCartaConducaoLightAsync(string keyword = "");
    Task<PaginatedResponse<CartaConducaoTableDTO>> GetCartaConducaoPaginatedAsync(CartaConducaoTableFilter filter);
    Task<Response<IEnumerable<CartaConducaoTableDTO>>> GetAllCartaConducaoAsync(CartaConducaoAllFilter filter);
    Task<Response<CartaConducaoDTO>> GetCartaConducaoAsync(Guid id);
    Task<Response<Guid>> CreateCartaConducaoAsync(CreateCartaConducaoRequest request);
    Task<Response<Guid>> UpdateCartaConducaoAsync(UpdateCartaConducaoRequest request, Guid id);
    Task<Response<Guid>> DeleteCartaConducaoAsync(Guid id);
    Task<Response<IEnumerable<Guid>>> DeleteMultipleCartaConducaoAsync(IEnumerable<Guid> ids);
  }
}
