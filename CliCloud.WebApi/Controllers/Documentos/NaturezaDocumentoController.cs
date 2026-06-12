using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Filters;

namespace CliCloud.WebApi.Controllers.Documentos
{
    [Route("client/documentos/[controller]")]
    [ApiController]
    public class NaturezaDocumentoController(INaturezaDocumentoService naturezaDocumentoService) : ControllerBase
    {
        private readonly INaturezaDocumentoService _naturezaDocumentoService = naturezaDocumentoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetNaturezaDocumentoAsync(string keyword = "")
        {
            Response<IEnumerable<NaturezaDocumentoDTO>> result = await _naturezaDocumentoService.GetNaturezaDocumentoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetNaturezaDocumentoLightAsync(string keyword = "")
        {
            Response<IEnumerable<NaturezaDocumentoLightDTO>> result = await _naturezaDocumentoService.GetNaturezaDocumentoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetNaturezaDocumentoPaginatedAsync(NaturezaDocumentoTableFilter filter)
        {
            PaginatedResponse<NaturezaDocumentoTableDTO> result = await _naturezaDocumentoService.GetNaturezaDocumentoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllNaturezaDocumentoAsync([FromBody] NaturezaDocumentoAllFilter? filter = null)
        {
            Response<IEnumerable<NaturezaDocumentoTableDTO>> result =
                await _naturezaDocumentoService.GetAllNaturezaDocumentoAsync(filter ?? new NaturezaDocumentoAllFilter());
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNaturezaDocumentoByIdAsync(Guid id)
        {
            Response<NaturezaDocumentoDTO> result = await _naturezaDocumentoService.GetNaturezaDocumentoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateNaturezaDocumentoAsync(CreateNaturezaDocumentoRequest request)
        {
            try
            {
                Response<Guid> result = await _naturezaDocumentoService.CreateNaturezaDocumentoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNaturezaDocumentoAsync(UpdateNaturezaDocumentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _naturezaDocumentoService.UpdateNaturezaDocumentoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNaturezaDocumentoAsync(Guid id)
        {
            try
            {
                Response<Guid> result = await _naturezaDocumentoService.DeleteNaturezaDocumentoAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleNaturezaDocumentoAsync([FromBody] DeleteMultipleNaturezaDocumentoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _naturezaDocumentoService.DeleteMultipleNaturezaDocumentoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
