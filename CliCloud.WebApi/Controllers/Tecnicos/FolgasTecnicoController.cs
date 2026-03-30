using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService;
using CliCloud.Application.Services.Tecnicos.FolgasTecnicoService.DTOs;

namespace CliCloud.WebApi.Controllers.Tecnicos
{
    [Route("client/tecnicos/[controller]")]
    [ApiController]
    public class FolgasTecnicoController(IFolgasTecnicoService service) : ControllerBase
    {
        [Authorize(Roles = "client")]
        [HttpGet("tecnico/{tecnicoId}")]
        public async Task<IActionResult> GetByTecnicoIdAsync(Guid tecnicoId)
        {
            var result = await service.GetByTecnicoIdAsync(tecnicoId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await service.GetAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateFolgasTecnicoRequest request)
        {
            try
            {
                var result = await service.CreateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(UpdateFolgasTecnicoRequest request, Guid id)
        {
            try
            {
                var result = await service.UpdateAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await service.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

