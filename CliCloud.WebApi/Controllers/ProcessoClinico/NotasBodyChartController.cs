using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.NotasBodyChartService.Filters;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class NotasBodyChartController : ControllerBase
    {
        private readonly INotasBodyChartService _notasBodyChartService;

        public NotasBodyChartController(INotasBodyChartService notasBodyChartService)
        {
            _notasBodyChartService = notasBodyChartService;
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetNotasBodyChartAsync(string keyword = "")
        {
            Response<IEnumerable<NotasBodyChartDTO>> result = await _notasBodyChartService.GetNotasBodyChartAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetNotasBodyChartLightAsync(string keyword = "")
        {
            Response<IEnumerable<NotasBodyChartLightDTO>> result = await _notasBodyChartService.GetNotasBodyChartLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetNotasBodyChartPaginatedAsync(NotasBodyChartTableFilter filter)
        {
            PaginatedResponse<NotasBodyChartTableDTO> result = await _notasBodyChartService.GetNotasBodyChartPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllNotasBodyChartAsync([FromBody] NotasBodyChartAllFilter filter)
        {
            Response<IEnumerable<NotasBodyChartTableDTO>> result = await _notasBodyChartService.GetAllNotasBodyChartAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotasBodyChartAsync(Guid id)
        {
            Response<NotasBodyChartDTO> result = await _notasBodyChartService.GetNotasBodyChartAsync(id);
            return Ok(result);
        }

        // single by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome")]
        public async Task<IActionResult> GetNotasBodyChartByNomeAsync(string nome)
        {
            Response<NotasBodyChartDTO> result = await _notasBodyChartService.GetNotasBodyChartByNomeAsync(nome);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateNotasBodyChartAsync(CreateNotasBodyChartRequest request)
        {
            Response<Guid> result = await _notasBodyChartService.CreateNotasBodyChartAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotasBodyChartAsync(UpdateNotasBodyChartRequest request, Guid id)
        {
            Response<Guid> result = await _notasBodyChartService.UpdateNotasBodyChartAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotasBodyChartAsync(Guid id)
        {
            Response<Guid> response = await _notasBodyChartService.DeleteNotasBodyChartAsync(id);
            return Ok(response);
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleNotasBodyChartAsync([FromBody] DeleteMultipleNotasBodyChartRequest request)
        {
            Response<IEnumerable<Guid>> response = await _notasBodyChartService.DeleteMultipleNotasBodyChartAsync(request.Ids);
            return Ok(response);
        }
    }
}
