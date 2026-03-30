using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class TipoAparelhoController(ITipoAparelhoService TipoAparelhoService) : ControllerBase
    {
        private readonly ITipoAparelhoService _TipoAparelhoService = TipoAparelhoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoAparelhoAsync(string keyword = "")
        {
            Response<IEnumerable<TipoAparelhoDTO>> result = await _TipoAparelhoService.GetTipoAparelhoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoAparelhoLightAsync(string keyword = "")
        {
          Response<IEnumerable<TipoAparelhoLightDTO>> result = await _TipoAparelhoService.GetTipoAparelhoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoAparelhoPaginatedAsync(TipoAparelhoTableFilter filter)
        {
            PaginatedResponse<TipoAparelhoTableDTO> result = await _TipoAparelhoService.GetTipoAparelhoPaginatedAsync(filter);
            return Ok(result);
        }

        // All TipoAparelhos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoAparelhoAsync([FromBody] TipoAparelhoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<TipoAparelhoTableDTO>> result = await _TipoAparelhoService.GetAllTipoAparelhoAsync(filter);
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
        public async Task<IActionResult> GetTipoAparelhoAsync(Guid id)
        {
            Response<TipoAparelhoDTO> result = await _TipoAparelhoService.GetTipoAparelhoAsync(id);
            return Ok(result);
        }

        // single by Designacao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("designacao/{designacao}")]
        public async Task<IActionResult> GetTipoAparelhoByDesignacaoAsync(string designacao)
        {
            try
            {
                Response<TipoAparelhoDTO> result = await _TipoAparelhoService.GetTipoAparelhoByDesignacaoAsync(designacao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoAparelhoAsync(CreateTipoAparelhoRequest request)
        {
            try
            {
                Response<Guid> result = await _TipoAparelhoService.CreateTipoAparelhoAsync(request);
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
        public async Task<IActionResult> UpdateTipoAparelhoAsync(UpdateTipoAparelhoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TipoAparelhoService.UpdateTipoAparelhoAsync(request, id);
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
        public async Task<IActionResult> DeleteTipoAparelhoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TipoAparelhoService.DeleteTipoAparelhoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTipoAparelhoAsync([FromBody] DeleteMultipleTipoAparelhoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TipoAparelhoService.DeleteMultipleTipoAparelhoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
