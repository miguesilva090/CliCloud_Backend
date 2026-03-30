using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.ConcelhoService;
using CliCloud.Application.Services.Utility.ConcelhoService.DTOs;
using CliCloud.Application.Services.Utility.ConcelhoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class ConcelhoController(IConcelhoService ConcelhoService) : ControllerBase
    {
        private readonly IConcelhoService _ConcelhoService = ConcelhoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetConcelhoAsync(string keyword = "")
        {
            Response<IEnumerable<ConcelhoDTO>> result = await _ConcelhoService.GetConcelhoAsync(keyword);
            return Ok(result);
        }

        //lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetConcelhoLightAsync(string keyword = "")
        {
          Response<IEnumerable<ConcelhoLightDTO>> result = await _ConcelhoService.GetConcelhoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetConcelhoPaginatedAsync(ConcelhoTableFilter filter)
        {
            PaginatedResponse<ConcelhoTableDTO> result = await _ConcelhoService.GetConcelhoPaginatedAsync(filter);
            return Ok(result);
        }

        // all Concelhos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllConcelhoAsync([FromBody] ConcelhoAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<ConcelhoTableDTO>> result = await _ConcelhoService.GetAllConcelhoAsync(filter ?? new ConcelhoAllFilter());
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
        public async Task<IActionResult> GetConcelhoAsync(Guid id)
        {
            Response<ConcelhoDTO> result = await _ConcelhoService.GetConcelhoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateConcelhoAsync(CreateConcelhoRequest request)
        {
            try
            {
                Response<Guid> result = await _ConcelhoService.CreateConcelhoAsync(request);
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
        public async Task<IActionResult> UpdateConcelhoAsync(UpdateConcelhoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ConcelhoService.UpdateConcelhoAsync(request, id);
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
        public async Task<IActionResult> DeleteConcelhoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ConcelhoService.DeleteConcelhoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleConcelhoAsync([FromBody] DeleteMultipleConcelhoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ConcelhoService.DeleteMultipleConcelhoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
