using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.RuaService;
using CliCloud.Application.Services.Utility.RuaService.DTOs;
using CliCloud.Application.Services.Utility.RuaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class RuaController(IRuaService RuaService) : ControllerBase
    {
        private readonly IRuaService _RuaService = RuaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetRuaAsync(string keyword = "")
        {
            Response<IEnumerable<RuaDTO>> result = await _RuaService.GetRuaAsync(keyword);
            return Ok(result);
        }

        //Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetRuaLightAsync(string keyword = "")
        {
          Response<IEnumerable<RuaLightDTO>> result = await _RuaService.GetRuaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetRuaPaginatedAsync(RuaTableFilter filter)
        {
            PaginatedResponse<RuaTableDTO> result = await _RuaService.GetRuaPaginatedAsync(filter);
            return Ok(result);
        }

        // All Ruas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllRuaAsync([FromBody] RuaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<RuaTableDTO>> result = await _RuaService.GetAllRuaAsync(filter ?? new RuaAllFilter());
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
        public async Task<IActionResult> GetRuaAsync(Guid id)
        {
            Response<RuaDTO> result = await _RuaService.GetRuaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateRuaAsync(CreateRuaRequest request)
        {
            try
            {
                Response<Guid> result = await _RuaService.CreateRuaAsync(request);
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
        public async Task<IActionResult> UpdateRuaAsync(UpdateRuaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _RuaService.UpdateRuaAsync(request, id);
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
        public async Task<IActionResult> DeleteRuaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _RuaService.DeleteRuaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Multiple Bulk 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleRuasAsync([FromBody] DeleteMultipleRuaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> response = await _RuaService.DeleteMultipleRuasAsync(request.Ids);
            return Ok(response);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
