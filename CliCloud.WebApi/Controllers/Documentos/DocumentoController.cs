using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Documentos.DocumentoService;
using CliCloud.Application.Services.Documentos.DocumentoService.DTOs;
using CliCloud.Application.Services.Documentos.DocumentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Documentos
{
    [Route("client/documentos/[controller]")]
    [ApiController]
    public class DocumentoController(IDocumentoService DocumentoService) : ControllerBase
    {
        private readonly IDocumentoService _DocumentoService = DocumentoService ;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentoAsync(string keyword = "")
        {
            Response<IEnumerable<DocumentoDTO>> result = await _DocumentoService.GetDocumentoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetDocumentoLightAsync(string keyword = "")
        {
          Response<IEnumerable<DocumentoLightDTO>> result = await _DocumentoService.GetDocumentoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetDocumentoPaginatedAsync(DocumentoTableFilter filter)
        {
            PaginatedResponse<DocumentoTableDTO> result = await _DocumentoService.GetDocumentoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Documentos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllDocumentoAsync([FromBody] DocumentoAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<DocumentoTableDTO>> result = await _DocumentoService.GetAllDocumentoAsync(filter ?? new DocumentoAllFilter());
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
        public async Task<IActionResult> GetDocumentoAsync(Guid id)
        {
            Response<DocumentoDTO> result = await _DocumentoService.GetDocumentoAsync(id);
            return Ok(result);
        }

        // Single by TipoDocumentoID and NumeroDocumento
        [Authorize(Roles = "client")]
        [HttpGet("tipo-numero/{tipoDocumentoId}/{numeroDocumento:int}")]
        public async Task<IActionResult> GetDocumentoByTipoNumeroAsync(Guid tipoDocumentoId, int numeroDocumento)
        {
          try
          {
            Response<DocumentoDTO> result = await _DocumentoService.GetDocumentoByTipoNumeroAsync(tipoDocumentoId, numeroDocumento);
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
        public async Task<IActionResult> CreateDocumentoAsync(CreateDocumentoRequest request)
        {
            try
            {
                Response<Guid> result = await _DocumentoService.CreateDocumentoAsync(request);
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
        public async Task<IActionResult> UpdateDocumentoAsync(UpdateDocumentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _DocumentoService.UpdateDocumentoAsync(request, id);
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
        public async Task<IActionResult> DeleteDocumentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _DocumentoService.DeleteDocumentoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleDocumentoAsync([FromBody] DeleteMultipleDocumentoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _DocumentoService.DeleteMultipleDocumentoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
