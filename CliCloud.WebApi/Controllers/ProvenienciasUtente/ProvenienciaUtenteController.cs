using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs;
using CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.ProvenienciasUtente
{
    [Route("client/proveniencias-utentes/[controller]")]
    [ApiController]
    public class ProvenienciaUtenteController(IProvenienciaUtenteService provenienciaUtenteService) : ControllerBase
    {
        private readonly IProvenienciaUtenteService _provenienciaUtenteService = provenienciaUtenteService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetProvenienciaUtenteAsync(string keyword = "")
        {
            Response<IEnumerable<ProvenienciaUtenteDTO>> result = await _provenienciaUtenteService.GetProvenienciaUtenteAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetProvenienciaUtenteLightAsync(string keyword = "")
        {
            Response<IEnumerable<ProvenienciaUtenteLightDTO>> result = await _provenienciaUtenteService.GetProvenienciaUtenteLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetProvenienciaUtentePaginatedAsync(ProvenienciaUtenteTableFilter filter)
        {
            PaginatedResponse<ProvenienciaUtenteTableDTO> result = await _provenienciaUtenteService.GetProvenienciaUtentePaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllProvenienciaUtenteAsync([FromBody] ProvenienciaUtenteAllFilter filter)
        {
            try
            {
                Response<IEnumerable<ProvenienciaUtenteTableDTO>> result = await _provenienciaUtenteService.GetAllProvenienciaUtenteAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProvenienciaUtenteAsync(Guid id)
        {
            Response<ProvenienciaUtenteDTO> result = await _provenienciaUtenteService.GetProvenienciaUtenteAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateProvenienciaUtenteAsync(CreateProvenienciaUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _provenienciaUtenteService.CreateProvenienciaUtenteAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProvenienciaUtenteAsync([FromRoute] Guid id, [FromBody] UpdateProvenienciaUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _provenienciaUtenteService.UpdateProvenienciaUtenteAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvenienciaUtenteAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _provenienciaUtenteService.DeleteProvenienciaUtenteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleProvenienciaUtenteAsync([FromBody] DeleteMultipleProvenienciaUtenteRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _provenienciaUtenteService.DeleteMultipleProvenienciaUtenteAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
