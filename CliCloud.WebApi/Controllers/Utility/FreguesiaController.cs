using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.FreguesiaService;
using CliCloud.Application.Services.Utility.FreguesiaService.DTOs;
using CliCloud.Application.Services.Utility.FreguesiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class FreguesiaController(IFreguesiaService FreguesiaService) : ControllerBase
    {
        private readonly IFreguesiaService _FreguesiaService = FreguesiaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetFreguesiaAsync(string keyword = "")
        {
            Response<IEnumerable<FreguesiaDTO>> result = await _FreguesiaService.GetFreguesiaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetFreguesiaLightAsync(string keyword = "")
        {
          Response<IEnumerable<FreguesiaLightDTO>> result = await _FreguesiaService.GetFreguesiaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetFreguesiaPaginatedAsync(FreguesiaTableFilter filter)
        {
            PaginatedResponse<FreguesiaTableDTO> result = await _FreguesiaService.GetFreguesiaPaginatedAsync(filter);
            return Ok(result);
        }

        //All Freguesias (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllFreguesiaAsync([FromBody] FreguesiaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<FreguesiaTableDTO>> result = await _FreguesiaService.GetAllFreguesiaAsync(filter ?? new FreguesiaAllFilter());
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
        public async Task<IActionResult> GetFreguesiaAsync(Guid id)
        {
            Response<FreguesiaDTO> result = await _FreguesiaService.GetFreguesiaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateFreguesiaAsync(CreateFreguesiaRequest request)
        {
            try
            {
                Response<Guid> result = await _FreguesiaService.CreateFreguesiaAsync(request);
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
        public async Task<IActionResult> UpdateFreguesiaAsync(UpdateFreguesiaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _FreguesiaService.UpdateFreguesiaAsync(request, id);
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
        public async Task<IActionResult> DeleteFreguesiaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _FreguesiaService.DeleteFreguesiaAsync(id);
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
        public async Task<IActionResult> DeleteMultipleFreguesiaAsync([FromBody] DeleteMultipleFreguesiaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _FreguesiaService.DeleteMultipleFreguesiaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
