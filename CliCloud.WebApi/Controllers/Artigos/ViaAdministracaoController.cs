using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.ViaAdministracaoService.Filters;

namespace CliCloud.WebApi.Controllers.Artigos
{
    [Route("client/artigos/[controller]")]
    [ApiController]
    public class ViaAdministracaoController(IViaAdministracaoService viaAdministracaoService) : ControllerBase
    {
        private readonly IViaAdministracaoService _viaAdministracaoService = viaAdministracaoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetViaAdministracaoAsync(string keyword = "")
        {
            Response<IEnumerable<ViaAdministracaoDTO>> result =
                await _viaAdministracaoService.GetViaAdministracaoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetViaAdministracaoPaginatedAsync(ViaAdministracaoTableFilter filter)
        {
            PaginatedResponse<ViaAdministracaoTableDTO> result =
                await _viaAdministracaoService.GetViaAdministracaoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetViaAdministracaoAsync(Guid id)
        {
            Response<ViaAdministracaoDTO> result =
                await _viaAdministracaoService.GetViaAdministracaoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateViaAdministracaoAsync(CreateViaAdministracaoRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _viaAdministracaoService.CreateViaAdministracaoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateViaAdministracaoAsync(
            [FromRoute] Guid id,
            [FromBody] UpdateViaAdministracaoRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _viaAdministracaoService.UpdateViaAdministracaoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteViaAdministracaoAsync(Guid id)
        {
            try
            {
                Response<Guid> result =
                    await _viaAdministracaoService.DeleteViaAdministracaoAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleViaAdministracaoAsync(
            [FromBody] DeleteMultipleViaAdministracaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result =
                    await _viaAdministracaoService.DeleteMultipleViaAdministracaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

