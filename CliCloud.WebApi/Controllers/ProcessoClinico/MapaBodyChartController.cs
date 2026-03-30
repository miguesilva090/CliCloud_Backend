using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Filters;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class MapaBodyChartController(IMapaBodyChartService mapaBodyChartService) : ControllerBase
    {
        private readonly IMapaBodyChartService _mapaBodyChartService = mapaBodyChartService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMapaBodyChartAsync(string keyword = "")
        {
            Response<IEnumerable<MapaBodyChartDTO>> result = await _mapaBodyChartService.GetMapaBodyChartAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMapaBodyChartLightAsync(string keyword = "")
        {
            Response<IEnumerable<MapaBodyChartLightDTO>> result = await _mapaBodyChartService.GetMapaBodyChartLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMapaBodyChartPaginatedAsync(MapaBodyChartTableFilter filter)
        {
            PaginatedResponse<MapaBodyChartTableDTO> result = await _mapaBodyChartService.GetMapaBodyChartPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMapaBodyChartAsync([FromBody] MapaBodyChartAllFilter filter)
        {
            Response<IEnumerable<MapaBodyChartTableDTO>> result = await _mapaBodyChartService.GetAllMapaBodyChartAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMapaBodyChartAsync(Guid id)
        {
            Response<MapaBodyChartDTO> result = await _mapaBodyChartService.GetMapaBodyChartAsync(id);
            return Ok(result);
        }

        // single by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome")]
        public async Task<IActionResult> GetMapaBodyChartByNomeAsync(string nome)
        {
            Response<MapaBodyChartDTO> result = await _mapaBodyChartService.GetMapaBodyChartByNomeAsync(nome);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMapaBodyChartAsync(CreateMapaBodyChartRequest request)
        {
            Response<Guid> result = await _mapaBodyChartService.CreateMapaBodyChartAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMapaBodyChartAsync(UpdateMapaBodyChartRequest request, Guid id)
        {
            Response<Guid> result = await _mapaBodyChartService.UpdateMapaBodyChartAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMapaBodyChartAsync(Guid id)
        {
            Response<Guid> response = await _mapaBodyChartService.DeleteMapaBodyChartAsync(id);
            return Ok(response);
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleMapaBodyChartAsync([FromBody] DeleteMultipleBodyChartRequest request)
        {
            Response<IEnumerable<Guid>> response = await _mapaBodyChartService.DeleteMultipleMapaBodyChartAsync(request.Ids);
            return Ok(response);
        }
    }
}
