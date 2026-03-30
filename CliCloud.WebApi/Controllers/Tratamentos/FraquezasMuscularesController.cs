using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs;
using CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class FraquezasMuscularesController(IFraquezasMuscularesService fraquezasMuscularesService) : ControllerBase
    {
        private readonly IFraquezasMuscularesService _fraquezasMuscularesService = fraquezasMuscularesService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetFraquezasMuscularesAsync(string keyword = "")
        {
            Response<IEnumerable<FraquezasMuscularesDTO>> result = await _fraquezasMuscularesService.GetFraquezasMuscularesAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetFraquezasMuscularesLightAsync(string keyword = "")
        {
            Response<IEnumerable<FraquezasMuscularesLightDTO>> result = await _fraquezasMuscularesService.GetFraquezasMuscularesLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetFraquezasMuscularesPaginatedAsync(FraquezasMuscularesTableFilter filter)
        {
            PaginatedResponse<FraquezasMuscularesTableDTO> result = await _fraquezasMuscularesService.GetFraquezasMuscularesPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllFraquezasMuscularesAsync([FromBody] FraquezasMuscularesAllFilter filter)
        {
            try
            {
                Response<IEnumerable<FraquezasMuscularesTableDTO>> result = await _fraquezasMuscularesService.GetAllFraquezasMuscularesAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFraquezasMuscularesAsync(Guid id)
        {
            Response<FraquezasMuscularesDTO> result = await _fraquezasMuscularesService.GetFraquezasMuscularesAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateFraquezasMuscularesAsync(CreateFraquezasMuscularesRequest request)
        {
            try
            {
                Response<Guid> result = await _fraquezasMuscularesService.CreateFraquezasMuscularesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFraquezasMuscularesAsync(UpdateFraquezasMuscularesRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _fraquezasMuscularesService.UpdateFraquezasMuscularesAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFraquezasMuscularesAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _fraquezasMuscularesService.DeleteFraquezasMuscularesAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleFraquezasMuscularesAsync([FromBody] DeleteMultipleFraquezasMuscularesRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _fraquezasMuscularesService.DeleteMultipleFraquezasMuscularesAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
