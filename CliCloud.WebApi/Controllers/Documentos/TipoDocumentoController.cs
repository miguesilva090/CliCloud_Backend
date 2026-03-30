using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Documentos.TipoDocumentoService;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.TipoDocumentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Documentos
{
    [Route("client/documentos/[controller]")]
    [ApiController]

    public class TipoDocumentoController(ITipoDocumentoService TipoDocumentoService) : ControllerBase
    {
        private readonly ITipoDocumentoService _TipoDocumentoService = TipoDocumentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoDocumentoAsync(string keyword = "")
        {
            Response<IEnumerable<TipoDocumentoDTO>> result = await _TipoDocumentoService.GetTipoDocumentoAsync(keyword);
            return Ok(result);
        }

        //Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoDocumentoLightAsync(string keyword = "")
        {
          Response<IEnumerable<TipoDocumentoLightDTO>> result = await _TipoDocumentoService.GetTipoDocumentoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoDocumentoPaginatedAsync(TipoDocumentoTableFilter filter)
        {
            PaginatedResponse<TipoDocumentoTableDTO> result = await _TipoDocumentoService.GetTipoDocumentoPaginatedAsync(filter);
            return Ok(result);
        }

        // All TipoDocumentos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoDocumentoAsync([FromBody] TipoDocumentoAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<TipoDocumentoTableDTO>> result = await _TipoDocumentoService.GetAllTipoDocumentoAsync(filter ?? new TipoDocumentoAllFilter());
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoDocumentoAsync(Guid id)
        {
            Response<TipoDocumentoDTO> result = await _TipoDocumentoService.GetTipoDocumentoAsync(id);
            return Ok(result);
        }

        //Single by Abreviatura 
        [Authorize(Roles = "client")]
        [HttpGet("abreviatura/{abreviatura}")]
        public async Task<IActionResult> GetTipoDocumentoByAbreviaturaAsync(string abreviatura)
        {
          try
          {
            Response<TipoDocumentoDTO> result = await _TipoDocumentoService.GetTipoDocumentoByAbreviaturaAsync(abreviatura);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoDocumentoAsync(CreateTipoDocumentoRequest request)
        {
            try
            {
                Response<Guid> result = await _TipoDocumentoService.CreateTipoDocumentoAsync(request);
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
        public async Task<IActionResult> UpdateTipoDocumentoAsync(UpdateTipoDocumentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TipoDocumentoService.UpdateTipoDocumentoAsync(request, id);
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
        public async Task<IActionResult> DeleteTipoDocumentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TipoDocumentoService.DeleteTipoDocumentoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTipoDocumentoAsync([FromBody] DeleteMultipleTipoDocumentoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TipoDocumentoService.DeleteMultipleTipoDocumentoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
