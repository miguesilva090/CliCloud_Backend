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


        // print
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}/print")]
        public async Task<IActionResult> GetDocumentoPrintAsync(Guid id)
        {
          var result = await _DocumentoService.GetDocumentoPrintAsync(id);
          return Ok(result);
        }

        // print original
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}/print/original")]
        public async Task<IActionResult> GetDocumentoPrintOriginalAsync(Guid id)
        {
          var result = await _DocumentoService.GetDocumentoPrintOriginalAsync(id);
          return Ok(result);
        }

        // Enviar por email
        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/email")]
        public async Task<IActionResult> EnviarDocumentoPorEmailAsync(Guid id, [FromBody] EnviarDocumentoEmailRequest request)
        {
          var result = await _DocumentoService.EnviarDocumentoPorEmailAsync(id, request);
          return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}/detalhes-admissoes")]
        public async Task<IActionResult> GetDocumentoDetalhesAdmissoesAsync(Guid id)
        {
          var result = await _DocumentoService.GetDocumentoDetalhesAdmissoesAsync(id);
          return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}/liquidacao-contexto")]
        public async Task<IActionResult> GetDocumentoLiquidacaoContextoAsync(Guid id)
        {
          var result = await _DocumentoService.GetDocumentoLiquidacaoContextoAsync(id);
          return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/liquidar")]
        public async Task<IActionResult> LiquidarDocumentoAsync(Guid id)
        {
          var result = await _DocumentoService.LiquidarDocumentoAsync(id);
          return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("{id:guid}/validacao-transporte")]
        public async Task<IActionResult> AtualizarValidacaoTransporteAsync(
            Guid id,
            [FromBody] AtualizarValidacaoTransporteRequest request)
        {
          var result = await _DocumentoService.AtualizarValidacaoTransporteAsync(id, request);
          return Ok(result);
        }
    }
}
