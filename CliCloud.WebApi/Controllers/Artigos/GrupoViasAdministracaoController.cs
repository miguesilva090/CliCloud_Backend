using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs;
using CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.Filters;

namespace CliCloud.WebApi.Controllers.Artigos
{
    [Route("client/artigos/[controller]")]
    [ApiController]
    public class GrupoViasAdministracaoController(IGrupoViasAdministracaoService service) : ControllerBase
    {
        private readonly IGrupoViasAdministracaoService _service = service;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGrupoViasAdministracaoAsync(string keyword = "")
        {
            Response<IEnumerable<GrupoViasAdministracaoDTO>> result =
                await _service.GetGrupoViasAdministracaoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGrupoViasAdministracaoPaginatedAsync(GrupoViasAdministracaoTableFilter filter)
        {
            PaginatedResponse<GrupoViasAdministracaoTableDTO> result =
                await _service.GetGrupoViasAdministracaoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGrupoViasAdministracaoAsync(Guid id)
        {
            Response<GrupoViasAdministracaoDTO> result =
                await _service.GetGrupoViasAdministracaoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGrupoViasAdministracaoAsync(CreateGrupoViasAdministracaoRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _service.CreateGrupoViasAdministracaoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGrupoViasAdministracaoAsync(
            [FromRoute] Guid id,
            [FromBody] UpdateGrupoViasAdministracaoRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _service.UpdateGrupoViasAdministracaoAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGrupoViasAdministracaoAsync(Guid id)
        {
            try
            {
                Response<Guid> result =
                    await _service.DeleteGrupoViasAdministracaoAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleGrupoViasAdministracaoAsync(
            [FromBody] DeleteMultipleGrupoViasAdministracaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result =
                    await _service.DeleteMultipleGrupoViasAdministracaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

