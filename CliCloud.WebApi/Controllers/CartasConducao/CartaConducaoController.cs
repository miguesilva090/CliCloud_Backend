using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs;
using CliCloud.Application.Services.CartasConducao.CartaConducaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.CartasConducao
{
    [Route("client/cartasconducao/[controller]")]
    [ApiController]
    public class CartaConducaoController(ICartaConducaoService CartaConducaoService) : ControllerBase
    {
        private readonly ICartaConducaoService _CartaConducaoService = CartaConducaoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCartaConducaoAsync(string keyword = "")
        {
            Response<IEnumerable<CartaConducaoDTO>> result = await _CartaConducaoService.GetCartaConducaoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCartaConducaoLightAsync(string keyword = "")
        {
            Response<IEnumerable<CartaConducaoLightDTO>> result = await _CartaConducaoService.GetCartaConducaoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCartaConducaoPaginatedAsync(CartaConducaoTableFilter filter)
        {
            PaginatedResponse<CartaConducaoTableDTO> result = await _CartaConducaoService.GetCartaConducaoPaginatedAsync(filter);
            return Ok(result);
        }

        // All CartaConducao (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCartaConducaoAsync([FromBody] CartaConducaoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<CartaConducaoTableDTO>> result = await _CartaConducaoService.GetAllCartaConducaoAsync(filter);
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
        public async Task<IActionResult> GetCartaConducaoAsync(Guid id)
        {
            Response<CartaConducaoDTO> result = await _CartaConducaoService.GetCartaConducaoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCartaConducaoAsync(CreateCartaConducaoRequest request)
        {
            try
            {
                Response<Guid> result = await _CartaConducaoService.CreateCartaConducaoAsync(request);
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
        public async Task<IActionResult> UpdateCartaConducaoAsync(UpdateCartaConducaoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _CartaConducaoService.UpdateCartaConducaoAsync(request, id);
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
        public async Task<IActionResult> DeleteCartaConducaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _CartaConducaoService.DeleteCartaConducaoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleCartaConducaoAsync([FromBody] DeleteMultipleCartaConducaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _CartaConducaoService.DeleteMultipleCartaConducaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
