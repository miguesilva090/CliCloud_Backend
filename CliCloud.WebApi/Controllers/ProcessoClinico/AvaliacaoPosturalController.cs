using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AvaliacaoPosturalController(IAvaliacaoPosturalService avaliacaoPosturalService) : ControllerBase
    {
        private readonly IAvaliacaoPosturalService _avaliacaoPosturalService = avaliacaoPosturalService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAvaliacaoPosturalAsync(string keyword = "")
        {
            Response<IEnumerable<AvaliacaoPosturalDTO>> result = await _avaliacaoPosturalService.GetAvaliacaoPosturalAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAvaliacaoPosturalLightAsync(string keyword = "")
        {
            Response<IEnumerable<AvaliacaoPosturalLightDTO>> result = await _avaliacaoPosturalService.GetAvaliacaoPosturalLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAvaliacaoPosturalPaginatedAsync([FromBody] AvaliacaoPosturalTableFilter filter)
        {
            PaginatedResponse<AvaliacaoPosturalDTO> result =
                await _avaliacaoPosturalService.GetAvaliacaoPosturalPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAvaliacaoPosturalAsync([FromBody] AvaliacaoPosturalAllFilter filter)
        {
            Response<IEnumerable<AvaliacaoPosturalTableDTO>> result =
                await _avaliacaoPosturalService.GetAllAvaliacaoPosturalAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAvaliacaoPosturalAsync(Guid id)
        {
            Response<AvaliacaoPosturalDTO> result = await _avaliacaoPosturalService.GetAvaliacaoPosturalAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAvaliacaoPosturalAsync([FromBody] CreateAvaliacaoPosturalRequest request)
        {
            Response<Guid> result = await _avaliacaoPosturalService.CreateAvaliacaoPosturalAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAvaliacaoPosturalAsync([FromBody] UpdateAvaliacaoPosturalRequest request, Guid id)
        {
            Response<Guid> result = await _avaliacaoPosturalService.UpdateAvaliacaoPosturalAsync(request, id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAvaliacaoPosturalAsync(Guid id)
        {
            Response<Guid> result = await _avaliacaoPosturalService.DeleteAvaliacaoPosturalAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAvaliacaoPosturalAsync([FromBody] IEnumerable<Guid> ids)
        {
            Response<IEnumerable<Guid>> result = await _avaliacaoPosturalService.DeleteMultipleAvaliacaoPosturalAsync(ids);
            return Ok(result);
        }
    }
}
