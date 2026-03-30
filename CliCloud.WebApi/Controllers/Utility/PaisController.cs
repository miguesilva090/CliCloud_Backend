using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.PaisService;
using CliCloud.Application.Services.Utility.PaisService.DTOs;
using CliCloud.Application.Services.Utility.PaisService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class PaisController(IPaisService PaisService) : ControllerBase
    {
        private readonly IPaisService _PaisService = PaisService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetPaisAsync(string keyword = "")
        {
            Response<IEnumerable<PaisDTO>> result = await _PaisService.GetPaisAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetPaisLightAsync(string keyword = "")
        {
          Response<IEnumerable<PaisLightDTO>> result = await _PaisService.GetPaisLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPaisPaginatedAsync(PaisTableFilter filter)
        {
            PaginatedResponse<PaisTableDTO> result = await _PaisService.GetPaisPaginatedAsync(filter);
            return Ok(result);
        }

        // All Paises (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllPaisAsync([FromBody] PaisAllFilter? filter = null)
        {
          try
          {
          Response<IEnumerable<PaisTableDTO>> result = await _PaisService.GetAllPaisAsync(filter ?? new PaisAllFilter());
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
        public async Task<IActionResult> GetPaisAsync(Guid id)
        {
            Response<PaisDTO> result = await _PaisService.GetPaisAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreatePaisAsync(CreatePaisRequest request)
        {
            try
            {
                Response<Guid> result = await _PaisService.CreatePaisAsync(request);
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
        public async Task<IActionResult> UpdatePaisAsync(UpdatePaisRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _PaisService.UpdatePaisAsync(request, id);
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
        public async Task<IActionResult> DeletePaisAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _PaisService.DeletePaisAsync(id);
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
        public async Task<IActionResult> DeleteMultiplePaisAsync([FromBody] DeleteMultiplePaisRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _PaisService.DeleteMultiplePaisAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
