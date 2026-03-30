using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class TensaoArterialController(ITensaoArterialService tensaoArterialService) : ControllerBase
    {
        private readonly ITensaoArterialService _tensaoArterialService = tensaoArterialService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTensaoArterialAsync(string keyword = "")
        {
            Response<IEnumerable<TensaoArterialDTO>> result = await _tensaoArterialService.GetTensaoArterialAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTensaoArterialLightAsync(string keyword = "")
        {
            Response<IEnumerable<TensaoArterialLightDTO>> result = await _tensaoArterialService.GetTensaoArterialLightAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTensaoArterialPaginatedAsync([FromBody] TensaoArterialTableFilter filter)
        {
            PaginatedResponse<TensaoArterialDTO> result =
                await _tensaoArterialService.GetTensaoArterialPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTensaoArterialAsync([FromBody] TensaoArterialAllFilter filter)
        {
            Response<IEnumerable<TensaoArterialTableDTO>> result =
                await _tensaoArterialService.GetAllTensaoArterialAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTensaoArterialAsync(Guid id)
        {
            Response<TensaoArterialDTO> result = await _tensaoArterialService.GetTensaoArterialAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTensaoArterialAsync([FromBody] CreateTensaoArterialRequest request)
        {
            Response<Guid> result = await _tensaoArterialService.CreateTensaoArterialAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTensaoArterialAsync([FromBody] UpdateTensaoArterialRequest request, Guid id)
        {
            Response<Guid> result = await _tensaoArterialService.UpdateTensaoArterialAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTensaoArterialAsync(Guid id)
        {
            Response<Guid> result = await _tensaoArterialService.DeleteTensaoArterialAsync(id);
            return Ok(result);
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleTensaoArterialAsync([FromBody] IEnumerable<Guid> ids)
        {
            Response<IEnumerable<Guid>> result = await _tensaoArterialService.DeleteMultipleTensaoArterialAsync(ids);
            return Ok(result);
        }
    }
}

