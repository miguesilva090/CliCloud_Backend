using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs;
using CliCloud.Application.Services.Exames.CategoriaProcedimentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Exames
{
    [Route("client/exames/[controller]")]
    [ApiController]
    public class CategoriaProcedimentoController(ICategoriaProcedimentoService categoriaProcedimentoService) : ControllerBase
    {
        private readonly ICategoriaProcedimentoService _categoriaProcedimentoService = categoriaProcedimentoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCategoriaProcedimentoAsync(string keyword = "")
        {
            Response<IEnumerable<CategoriaProcedimentoDTO>> result = await _categoriaProcedimentoService.GetCategoriaProcedimentoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCategoriaProcedimentoLightAsync(string keyword = "")
        {
            Response<IEnumerable<CategoriaProcedimentoLightDTO>> result = await _categoriaProcedimentoService.GetCategoriaProcedimentoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCategoriaProcedimentoPaginatedAsync(CategoriaProcedimentoTableFilter filter)
        {
            PaginatedResponse<CategoriaProcedimentoTableDTO> result = await _categoriaProcedimentoService.GetCategoriaProcedimentoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCategoriaProcedimentoAsync([FromBody] CategoriaProcedimentoAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<CategoriaProcedimentoTableDTO>> result = await _categoriaProcedimentoService.GetAllCategoriaProcedimentoAsync(filter ?? new CategoriaProcedimentoAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaProcedimentoAsync(Guid id)
        {
            Response<CategoriaProcedimentoDTO> result = await _categoriaProcedimentoService.GetCategoriaProcedimentoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoriaProcedimentoAsync(CreateCategoriaProcedimentoRequest request)
        {
            try
            {
                Response<Guid> result = await _categoriaProcedimentoService.CreateCategoriaProcedimentoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoriaProcedimentoAsync(UpdateCategoriaProcedimentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _categoriaProcedimentoService.UpdateCategoriaProcedimentoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaProcedimentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _categoriaProcedimentoService.DeleteCategoriaProcedimentoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleCategoriaProcedimentoAsync([FromBody] DeleteMultipleCategoriaProcedimentoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _categoriaProcedimentoService.DeleteMultipleCategoriaProcedimentoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
