using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Servicos.TipoServicoService;
using CliCloud.Application.Services.Servicos.TipoServicoService.DTOs;
using CliCloud.Application.Services.Servicos.TipoServicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Servicos
{
    [Route("client/servicos/[controller]")]
    [ApiController]
    public class TipoServicoController(ITipoServicoService TipoServicoService) : ControllerBase
    {
        private readonly ITipoServicoService _TipoServicoService = TipoServicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoServicoAsync(string keyword = "")
        {
            Response<IEnumerable<TipoServicoDTO>> result = await _TipoServicoService.GetTipoServicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoServicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<TipoServicoLightDTO>> result = await _TipoServicoService.GetTipoServicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoServicoPaginatedAsync(TipoServicoTableFilter filter)
        {
            PaginatedResponse<TipoServicoTableDTO> result = await _TipoServicoService.GetTipoServicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All TipoServicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoServicoAsync([FromBody] TipoServicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<TipoServicoTableDTO>> result = await _TipoServicoService.GetAllTipoServicoAsync(filter);
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
        public async Task<IActionResult> GetTipoServicoAsync(Guid id)
        {
            Response<TipoServicoDTO> result = await _TipoServicoService.GetTipoServicoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoServicoAsync(CreateTipoServicoRequest request)
        {
            try
            {
                Response<Guid> result = await _TipoServicoService.CreateTipoServicoAsync(request);
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
        public async Task<IActionResult> UpdateTipoServicoAsync(UpdateTipoServicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TipoServicoService.UpdateTipoServicoAsync(request, id);
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
        public async Task<IActionResult> DeleteTipoServicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TipoServicoService.DeleteTipoServicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTipoServicoAsync([FromBody] DeleteMultipleTipoServicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TipoServicoService.DeleteMultipleTipoServicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
