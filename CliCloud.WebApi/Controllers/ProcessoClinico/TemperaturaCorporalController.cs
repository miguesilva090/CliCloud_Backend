using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TemperaturaCorporalService;
using CliCloud.Application.Services.TemperaturaCorporalService.DTOs;
using CliCloud.Application.Services.TemperaturaCorporalService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class TemperaturaCorporalController(ITemperaturaCorporalService temperaturaCorporalService) : ControllerBase
    {
        private readonly ITemperaturaCorporalService _temperaturaCorporalService = temperaturaCorporalService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTemperaturaCorporalAsync(string keyword = "")
        {
            Response<IEnumerable<TemperaturaCorporalDTO>> result = await _temperaturaCorporalService.GetTemperaturaCorporalAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTemperaturaCorporalPaginatedAsync([FromBody] TemperaturaCorporalTableFilter filter)
        {
            PaginatedResponse<TemperaturaCorporalDTO> result =
                await _temperaturaCorporalService.GetTemperaturaCorporalPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTemperaturaCorporalAsync(Guid id)
        {
            Response<TemperaturaCorporalDTO> result = await _temperaturaCorporalService.GetTemperaturaCorporalAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTemperaturaCorporalAsync([FromBody] CreateTemperaturaCorporalRequest request)
        {
            Response<Guid> result = await _temperaturaCorporalService.CreateTemperaturaCorporalAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTemperaturaCorporalAsync([FromBody] UpdateTemperaturaCorporalRequest request, Guid id)
        {
            Response<Guid> result = await _temperaturaCorporalService.UpdateTemperaturaCorporalAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTemperaturaCorporalAsync(Guid id)
        {
            Response<Guid> result = await _temperaturaCorporalService.DeleteTemperaturaCorporalAsync(id);
            return Ok(result);
        }
    }
}

