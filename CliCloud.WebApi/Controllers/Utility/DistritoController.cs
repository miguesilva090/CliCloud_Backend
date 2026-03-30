using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.DistritoService;
using CliCloud.Application.Services.Utility.DistritoService.DTOs;
using CliCloud.Application.Services.Utility.DistritoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class DistritoController( IDistritoService DistritoService) : ControllerBase
    {
        private readonly IDistritoService _DistritoService = DistritoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetDistritoAsync(string keyword = "")
        {
            Response<IEnumerable<DistritoDTO>> result = await _DistritoService.GetDistritoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetDistritosLightAsync(string keyword = "")
        {
          Response<IEnumerable<DistritoLightDTO>> result = await _DistritoService.GetDistritoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetDistritoPaginatedAsync(DistritoTableFilter filter)
        {
            PaginatedResponse<DistritoTableDTO> result = await _DistritoService.GetDistritoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Distritos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllDistritoAsync ( [FromBody] DistritoAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<DistritoTableDTO>> result = await _DistritoService.GetAllDistritoAsync(filter ?? new DistritoAllFilter());
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
        public async Task<IActionResult> GetDistritoAsync(Guid id)
        {
            Response<DistritoDTO> result = await _DistritoService.GetDistritoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateDistritoAsync(CreateDistritoRequest request)
        {
            try
            {
                Response<Guid> result = await _DistritoService.CreateDistritoAsync(request);
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
        public async Task<IActionResult> UpdateDistritoAsync(UpdateDistritoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _DistritoService.UpdateDistritoAsync(request, id);
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
        public async Task<IActionResult> DeleteDistritoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _DistritoService.DeleteDistritoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteMultipleDistritoAsync([FromBody] DeleteMultipleDistritoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> response = await _DistritoService.DeleteMultipleDistritoAsync(request.Ids);
            return Ok(response);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
