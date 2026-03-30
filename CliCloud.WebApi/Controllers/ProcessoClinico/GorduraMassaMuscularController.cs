using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class GorduraMassaMuscularController(IGorduraMassaMuscularService gorduraMassaMuscularService) : ControllerBase
    {
        private readonly IGorduraMassaMuscularService _gorduraMassaMuscularService = gorduraMassaMuscularService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetGorduraMassaMuscularAsync(string keyword = "")
        {
            Response<IEnumerable<GorduraMassaMuscularDTO>> result = await _gorduraMassaMuscularService.GetGorduraMassaMuscularAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetGorduraMassaMuscularLightAsync(string keyword = "")
        {
            Response<IEnumerable<GorduraMassaMuscularLightDTO>> result = await _gorduraMassaMuscularService.GetGorduraMassaMuscularLightAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetGorduraMassaMuscularPaginatedAsync([FromBody] GorduraMassaMuscularTableFilter filter)
        {
            PaginatedResponse<GorduraMassaMuscularDTO> result =
                await _gorduraMassaMuscularService.GetGorduraMassaMuscularPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllGorduraMassaMuscularAsync([FromBody] GorduraMassaMuscularAllFilter filter)
        {
            Response<IEnumerable<GorduraMassaMuscularTableDTO>> result =
                await _gorduraMassaMuscularService.GetAllGorduraMassaMuscularAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGorduraMassaMuscularAsync(Guid id)
        {
            Response<GorduraMassaMuscularDTO> result = await _gorduraMassaMuscularService.GetGorduraMassaMuscularAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateGorduraMassaMuscularAsync([FromBody] CreateGorduraMassaMuscularRequest request)
        {
            Response<Guid> result = await _gorduraMassaMuscularService.CreateGorduraMassaMuscularAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGorduraMassaMuscularAsync([FromBody] UpdateGorduraMassaMuscularRequest request, Guid id)
        {
            Response<Guid> result = await _gorduraMassaMuscularService.UpdateGorduraMassaMuscularAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGorduraMassaMuscularAsync(Guid id)
        {
            Response<Guid> result = await _gorduraMassaMuscularService.DeleteGorduraMassaMuscularAsync(id);
            return Ok(result);
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleGorduraMassaMuscularAsync([FromBody] IEnumerable<Guid> ids)
        {
            Response<IEnumerable<Guid>> result = await _gorduraMassaMuscularService.DeleteMultipleGorduraMassaMuscularAsync(ids);
            return Ok(result);
        }
    }
}
