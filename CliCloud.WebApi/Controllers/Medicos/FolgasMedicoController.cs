using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Medicos.FolgasMedicoService;
using CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class FolgasMedicoController(IFolgasMedicoService service) : ControllerBase
    {
        [Authorize(Roles = "client")]
        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetByMedicoIdAsync(Guid medicoId)
        {
            var result = await service.GetByMedicoIdAsync(medicoId);
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
        public async Task<IActionResult> CreateAsync(CreateFolgasMedicoRequest request)
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
        public async Task<IActionResult> UpdateAsync(UpdateFolgasMedicoRequest request, Guid id)
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
