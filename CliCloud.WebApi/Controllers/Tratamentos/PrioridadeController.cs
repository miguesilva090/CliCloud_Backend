using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.PrioridadeService;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs;
using CliCloud.Application.Services.Tratamentos.PrioridadeService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class PrioridadeController(IPrioridadeService prioridadeService) : ControllerBase
    {
        private readonly IPrioridadeService _prioridadeService = prioridadeService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetPrioridadeAsync(string keyword = "")
        {
            Response<IEnumerable<PrioridadeDTO>> result = await _prioridadeService.GetPrioridadeAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetPrioridadeLightAsync(string keyword = "")
        {
            Response<IEnumerable<PrioridadeLightDTO>> result = await _prioridadeService.GetPrioridadeLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPrioridadePaginatedAsync(PrioridadeTableFilter filter)
        {
            PaginatedResponse<PrioridadeTableDTO> result = await _prioridadeService.GetPrioridadePaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllPrioridadeAsync([FromBody] PrioridadeAllFilter filter)
        {
            try
            {
                Response<IEnumerable<PrioridadeTableDTO>> result = await _prioridadeService.GetAllPrioridadeAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrioridadeAsync(Guid id)
        {
            Response<PrioridadeDTO> result = await _prioridadeService.GetPrioridadeAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreatePrioridadeAsync(CreatePrioridadeRequest request)
        {
            try
            {
                Response<Guid> result = await _prioridadeService.CreatePrioridadeAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrioridadeAsync(UpdatePrioridadeRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _prioridadeService.UpdatePrioridadeAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrioridadeAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _prioridadeService.DeletePrioridadeAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultiplePrioridadeAsync([FromBody] DeleteMultiplePrioridadeRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _prioridadeService.DeleteMultiplePrioridadeAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
