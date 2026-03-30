using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.CodigoPostalService;
using CliCloud.Application.Services.Utility.CodigoPostalService.DTOs;
using CliCloud.Application.Services.Utility.CodigoPostalService.Filters;
using CliCloud.Application.Common.Wrapper;



namespace CliCloud.WebApi.Controllers.Utility
{
    [Route("client/utility/[controller]")]
    [ApiController]
    public class CodigoPostalController(ICodigoPostalService CodigoPostalService) : ControllerBase
    {
        private readonly ICodigoPostalService _CodigoPostalService = CodigoPostalService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCodigoPostalAsync(string keyword = "")
        {
            Response<IEnumerable<CodigoPostalDTO>> result = await _CodigoPostalService.GetCodigoPostalAsync(keyword);
            return Ok(result);
        }
        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCodigoPostalLightAsync(string keyword = "")
        {
          Response<IEnumerable<CodigoPostalLightDTO>> result = await _CodigoPostalService.GetCodigoPostalLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCodigoPostalPaginatedAsync(CodigoPostalTableFilter filter)
        {
            PaginatedResponse<CodigoPostalTableDTO> result = await _CodigoPostalService.GetCodigoPostalPaginatedAsync(filter);
            return Ok(result);
        }

        // all Codigos Postais (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCodigoPostalAsync([FromBody]CodigoPostalAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<CodigoPostalTableDTO>> result = await _CodigoPostalService.GetAllCodigoPostalAsync(filter ?? new CodigoPostalAllFilter());
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
        public async Task<IActionResult> GetCodigoPostalAsync(Guid id)
        {
            Response<CodigoPostalDTO> result = await _CodigoPostalService.GetCodigoPostalAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCodigoPostalAsync(CreateCodigoPostalRequest request)
        {
            try
            {
                Response<Guid> result = await _CodigoPostalService.CreateCodigoPostalAsync(request);
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
        public async Task<IActionResult> UpdateCodigoPostalAsync(UpdateCodigoPostalRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _CodigoPostalService.UpdateCodigoPostalAsync(request, id);
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
        public async Task<IActionResult> DeleteCodigoPostalAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _CodigoPostalService.DeleteCodigoPostalAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delete multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleCodigoPostalAsync([FromBody] DeleteMultipleCodigoPostalRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _CodigoPostalService.DeleteMultipleCodigoPostalAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
