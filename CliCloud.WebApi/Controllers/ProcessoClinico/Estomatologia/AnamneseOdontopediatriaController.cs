using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.AnamneseOdontopediatriaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class AnamneseOdontopediatriaController(IAnamneseOdontopediatriaService anamneseOdontopediatriaService) : ControllerBase
    {
        private readonly IAnamneseOdontopediatriaService _anamneseOdontopediatriaService = anamneseOdontopediatriaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAnamneseOdontopediatriaAsync(string keyword = "")
        {
            Response<IEnumerable<AnamneseOdontopediatriaDTO>> result = await _anamneseOdontopediatriaService.GetAnamneseOdontopediatriaAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAnamneseOdontopediatriaPaginatedAsync(AnamneseOdontopediatriaTableFilter filter)
        {
            PaginatedResponse<AnamneseOdontopediatriaDTO> result = await _anamneseOdontopediatriaService.GetAnamneseOdontopediatriaPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnamneseOdontopediatriaAsync(Guid id)
        {
            Response<AnamneseOdontopediatriaDTO> result = 
                await _anamneseOdontopediatriaService.GetAnamneseOdontopediatriaAsync(id);
            return Ok(result);
        }

        // single by utente
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId:guid}")]
        public async Task<IActionResult> GetAnamneseOdontopediatriaByUtenteAsync(Guid utenteId)
        {
            Response<AnamneseOdontopediatriaDTO?> result = 
                await _anamneseOdontopediatriaService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAnamneseOdontopediatriaAsync([FromBody] CreateAnamneseOdontopediatriaRequest request)
        {
            try
            {
                Response<Guid> result = await _anamneseOdontopediatriaService.CreateAnamneseOdontopediatriaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAnamneseOdontopediatriaAsync([FromBody] UpdateAnamneseOdontopediatriaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _anamneseOdontopediatriaService.UpdateAnamneseOdontopediatriaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAnamneseOdontopediatriaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _anamneseOdontopediatriaService.DeleteAnamneseOdontopediatriaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
