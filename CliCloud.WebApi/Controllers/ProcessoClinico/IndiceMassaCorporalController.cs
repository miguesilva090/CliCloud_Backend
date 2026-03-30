using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.IndiceMassaCorporalService;
using CliCloud.Application.Services.IndiceMassaCorporalService.DTOs;
using CliCloud.Application.Services.IndiceMassaCorporalService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class IndiceMassaCorporalController(IIndiceMassaCorporalService indiceMassaCorporalService) : ControllerBase
    {
        private readonly IIndiceMassaCorporalService _indiceMassaCorporalService = indiceMassaCorporalService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetIndiceMassaCorporalAsync(string keyword = "")
        {
            Response<IEnumerable<IndiceMassaCorporalDTO>> result =
                await _indiceMassaCorporalService.GetIndiceMassaCorporalAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetIndiceMassaCorporalPaginatedAsync([FromBody] IndiceMassaCorporalTableFilter filter)
        {
            PaginatedResponse<IndiceMassaCorporalDTO> result =
                await _indiceMassaCorporalService.GetIndiceMassaCorporalPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetIndiceMassaCorporalAsync(Guid id)
        {
            Response<IndiceMassaCorporalDTO> result =
                await _indiceMassaCorporalService.GetIndiceMassaCorporalAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateIndiceMassaCorporalAsync([FromBody] CreateIndiceMassaCorporalRequest request)
        {
            Response<Guid> result = await _indiceMassaCorporalService.CreateIndiceMassaCorporalAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateIndiceMassaCorporalAsync([FromBody] UpdateIndiceMassaCorporalRequest request, Guid id)
        {
            Response<Guid> result = await _indiceMassaCorporalService.UpdateIndiceMassaCorporalAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteIndiceMassaCorporalAsync(Guid id)
        {
            Response<Guid> result = await _indiceMassaCorporalService.DeleteIndiceMassaCorporalAsync(id);
            return Ok(result);
        }
    }
}

