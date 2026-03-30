using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Seguradoras.SeguradoraService;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Seguradoras
{
    [Route("client/seguradoras/[controller]")]
    [ApiController]
    public class SeguradoraController(ISeguradoraService SeguradoraService) : ControllerBase
    {
        private readonly ISeguradoraService _SeguradoraService = SeguradoraService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetSeguradoraAsync(string keyword = "")
        {
            Response<IEnumerable<SeguradoraDTO>> result = await _SeguradoraService.GetSeguradoraAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetSeguradoraLightAsync(string keyword = "")
        {
          Response<IEnumerable<SeguradoraLightDTO>> result = await _SeguradoraService.GetSeguradoraLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetSeguradoraPaginatedAsync(SeguradoraTableFilter filter)
        {
            PaginatedResponse<SeguradoraTableDTO> result = await _SeguradoraService.GetSeguradoraPaginatedAsync(filter);
            return Ok(result);
        }

        // All Seguradoras (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllSeguradoraAsync([FromBody] SeguradoraAllFilter filter)
        {
          try
          {
            Response<IEnumerable<SeguradoraTableDTO>> result = await _SeguradoraService.GetAllSeguradoraAsync(filter);
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
        public async Task<IActionResult> GetSeguradoraAsync(Guid id)
        {
            Response<SeguradoraDTO> result = await _SeguradoraService.GetSeguradoraAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateSeguradoraAsync(CreateSeguradoraRequest request)
        {
            try
            {
                Response<Guid> result = await _SeguradoraService.CreateSeguradoraAsync(request);
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
        public async Task<IActionResult> UpdateSeguradoraAsync(UpdateSeguradoraRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _SeguradoraService.UpdateSeguradoraAsync(request, id);
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
        public async Task<IActionResult> DeleteSeguradoraAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _SeguradoraService.DeleteSeguradoraAsync(id);
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
        public async Task<IActionResult> DeleteMultipleSeguradoraAsync([FromBody] DeleteMultipleSeguradoraRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _SeguradoraService.DeleteMultipleSeguradoraAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
