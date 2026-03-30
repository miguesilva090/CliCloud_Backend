using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.AlergiaUtenteService;
using CliCloud.Application.Services.AlergiaUtenteService.DTOs;
using CliCloud.Application.Services.AlergiaUtenteService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Alergias
{
    [Route("client/alergias/[controller]")]
    [ApiController]
    public class AlergiaUtenteController(IAlergiaUtenteService alergiaUtenteService) : ControllerBase
    {
        private readonly IAlergiaUtenteService _alergiaUtenteService = alergiaUtenteService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAlergiaUtenteAsync(string keyword = "")
        {
            Response<IEnumerable<AlergiaUtenteDTO>> result = await _alergiaUtenteService.GetAlergiaUtenteAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAlergiaUtentePaginatedAsync(AlergiaUtenteTableFilter filter)
        {
            PaginatedResponse<AlergiaUtenteDTO> result = await _alergiaUtenteService.GetAlergiaUtentePaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlergiaUtenteAsync(Guid id)
        {
            Response<AlergiaUtenteDTO> result = await _alergiaUtenteService.GetAlergiaUtenteAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAlergiaUtenteAsync(CreateAlergiaUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiaUtenteService.CreateAlergiaUtenteAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlergiaUtenteAsync(Guid id, [FromBody] UpdateAlergiaUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _alergiaUtenteService.UpdateAlergiaUtenteAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlergiaUtenteAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _alergiaUtenteService.DeleteAlergiaUtenteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
