using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.GlicemiaCapilarService;
using CliCloud.Application.Services.GlicemiaCapilarService.DTOs;
using CliCloud.Application.Services.GlicemiaCapilarService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class GlicemiaCapilarController(IGlicemiaCapilarService glicemiaCapilarService) : ControllerBase
    {
        private readonly IGlicemiaCapilarService _glicemiaCapilarService = glicemiaCapilarService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGlicemiaCapilarAsync(string keyword = "")
        {
            Response<IEnumerable<GlicemiaCapilarDTO>> result = await _glicemiaCapilarService.GetGlicemiaCapilarAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGlicemiaCapilarPaginatedAsync([FromBody] GlicemiaCapilarTableFilter filter)
        {
            PaginatedResponse<GlicemiaCapilarDTO> result =
                await _glicemiaCapilarService.GetGlicemiaCapilarPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGlicemiaCapilarAsync(Guid id)
        {
            Response<GlicemiaCapilarDTO> result = await _glicemiaCapilarService.GetGlicemiaCapilarAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGlicemiaCapilarAsync([FromBody] CreateGlicemiaCapilarRequest request)
        {
            Response<Guid> result = await _glicemiaCapilarService.CreateGlicemiaCapilarAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGlicemiaCapilarAsync([FromBody] UpdateGlicemiaCapilarRequest request, Guid id)
        {
            Response<Guid> result = await _glicemiaCapilarService.UpdateGlicemiaCapilarAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGlicemiaCapilarAsync(Guid id)
        {
            Response<Guid> result = await _glicemiaCapilarService.DeleteGlicemiaCapilarAsync(id);
            return Ok(result);
        }
    }
}

