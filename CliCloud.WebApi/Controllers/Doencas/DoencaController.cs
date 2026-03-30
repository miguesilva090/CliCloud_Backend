using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Doencas.DoencaService;
using CliCloud.Application.Services.Doencas.DoencaService.DTOs;
using CliCloud.Application.Services.Doencas.DoencaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Doencas
{
    /// <summary>
    /// API para Doenças (ICD-11). Permite ver, editar e eliminar. Não permite adicionar (dados importados via CliCloud.ICDImport).
    /// </summary>
    [Route("client/doencas/[controller]")]
    [ApiController]
    public class DoencaController(IDoencaService doencaService) : ControllerBase
    {
        private readonly IDoencaService _doencaService = doencaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetDoencasAsync(string keyword = "")
        {
            Response<IEnumerable<DoencaDTO>> result = await _doencaService.GetDoencasAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetDoencasPaginatedAsync(DoencaTableFilter filter)
        {
            PaginatedResponse<DoencaDTO> result = await _doencaService.GetDoencasPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoencaAsync(Guid id)
        {
            Response<DoencaDTO> result = await _doencaService.GetDoencaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoencaAsync([FromRoute] Guid id, [FromBody] UpdateDoencaRequest request)
        {
            try
            {
                Response<Guid> result = await _doencaService.UpdateDoencaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoencaAsync(Guid id)
        {
            try
            {
                Response<Guid> result = await _doencaService.DeleteDoencaAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
