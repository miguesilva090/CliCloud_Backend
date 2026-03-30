using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.CartasConducao
{
    [Route("client/cartasconducao/[controller]")]
    [ApiController]
    public class CartaConducaoRestricoesController(ICartaConducaoRestricoesService CartaConducaoRestricoesService) : ControllerBase
    {
        private readonly ICartaConducaoRestricoesService _CartaConducaoRestricoesService = CartaConducaoRestricoesService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCartaConducaoRestricoesAsync(string keyword = "")
        {
            Response<IEnumerable<CartaConducaoRestricoesDTO>> result = await _CartaConducaoRestricoesService.GetCartaConducaoRestricoesAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCartaConducaoRestricoesLightAsync(string keyword = "")
        {
            Response<IEnumerable<CartaConducaoRestricoesLightDTO>> result = await _CartaConducaoRestricoesService.GetCartaConducaoRestricoesLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCartaConducaoRestricoesPaginatedAsync(CartaConducaoRestricoesTableFilter filter)
        {
            PaginatedResponse<CartaConducaoRestricoesTableDTO> result = await _CartaConducaoRestricoesService.GetCartaConducaoRestricoesPaginatedAsync(filter);
            return Ok(result);
        }

        // All CartaConducaoRestricoes (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCartaConducaoRestricoesAsync([FromBody] CartaConducaoRestricoesAllFilter filter)
        {
            try
            {
                Response<IEnumerable<CartaConducaoRestricoesTableDTO>> result = await _CartaConducaoRestricoesService.GetAllCartaConducaoRestricoesAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartaConducaoRestricoesAsync(Guid id)
        {
            Response<CartaConducaoRestricoesDTO> result = await _CartaConducaoRestricoesService.GetCartaConducaoRestricoesAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCartaConducaoRestricoesAsync(CreateCartaConducaoRestricoesRequest request)
        {
            try
            {
                Response<Guid> result = await _CartaConducaoRestricoesService.CreateCartaConducaoRestricoesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartaConducaoRestricoesAsync(UpdateCartaConducaoRestricoesRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _CartaConducaoRestricoesService.UpdateCartaConducaoRestricoesAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartaConducaoRestricoesAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _CartaConducaoRestricoesService.DeleteCartaConducaoRestricoesAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleCartaConducaoRestricoesAsync([FromBody] DeleteMultipleCartaConducaoRestricoesRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _CartaConducaoRestricoesService.DeleteMultipleCartaConducaoRestricoesAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
