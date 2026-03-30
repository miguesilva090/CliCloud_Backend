using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AvaliacaoAntropometricaController(IAvaliacaoAntropometricaService avaliacaoAntropometricaService) : ControllerBase
    {
        private readonly IAvaliacaoAntropometricaService _avaliacaoAntropometricaService = avaliacaoAntropometricaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAvaliacaoAntropometricaAsync(string keyword = "")
        {
            Response<IEnumerable<AvaliacaoAntropometricaDTO>> result = await _avaliacaoAntropometricaService.GetAvaliacaoAntropometricaAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAvaliacaoAntropometricaLightAsync(string keyword = "")
        {
            Response<IEnumerable<AvaliacaoAntropometricaLightDTO>> result = await _avaliacaoAntropometricaService.GetAvaliacaoAntropometricaLightAsync(keyword);
            return Ok(result);
        }

        // paginated list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAvaliacaoAntropometricaPaginatedAsync([FromBody] AvaliacaoAntropometricaTableFilter filter)
        {
            PaginatedResponse<AvaliacaoAntropometricaDTO> result =
                await _avaliacaoAntropometricaService.GetAvaliacaoAntropometricaPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAvaliacaoAntropometricaAsync([FromBody] AvaliacaoAntropometricaAllFilter filter)
        {
            Response<IEnumerable<AvaliacaoAntropometricaTableDTO>> result =
                await _avaliacaoAntropometricaService.GetAllAvaliacaoAntropometricaAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAvaliacaoAntropometricaAsync(Guid id)
        {
            Response<AvaliacaoAntropometricaDTO> result = await _avaliacaoAntropometricaService.GetAvaliacaoAntropometricaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAvaliacaoAntropometricaAsync([FromBody] CreateAvaliacaoAntropometricaRequest request)
        {
            Response<Guid> result = await _avaliacaoAntropometricaService.CreateAvaliacaoAntropometricaAsync(request);
            return Ok(result);
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAvaliacaoAntropometricaAsync([FromBody] UpdateAvaliacaoAntropometricaRequest request, Guid id)
        {
            Response<Guid> result = await _avaliacaoAntropometricaService.UpdateAvaliacaoAntropometricaAsync(request, id);
            return Ok(result);
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAvaliacaoAntropometricaAsync(Guid id)
        {
            Response<Guid> result = await _avaliacaoAntropometricaService.DeleteAvaliacaoAntropometricaAsync(id);
            return Ok(result);
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAvaliacaoAntropometricaAsync([FromBody] IEnumerable<Guid> ids)
        {
            Response<IEnumerable<Guid>> result = await _avaliacaoAntropometricaService.DeleteMultipleAvaliacaoAntropometricaAsync(ids);
            return Ok(result);
        }
    }
}
