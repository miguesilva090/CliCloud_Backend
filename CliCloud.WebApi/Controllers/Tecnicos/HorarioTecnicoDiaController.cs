using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tecnicos
{
    [Route("client/tecnicos/[controller]")]
    [ApiController]
    public class HorarioTecnicoDiaController(IHorarioTecnicoDiaService HorarioTecnicoDiaService) : ControllerBase
    {
        private readonly IHorarioTecnicoDiaService _HorarioTecnicoDiaService = HorarioTecnicoDiaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHorarioTecnicoDiaAsync(string keyword = "")
        {
            Response<IEnumerable<HorarioTecnicoDiaDTO>> result = await _HorarioTecnicoDiaService.GetHorarioTecnicoDiaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHorarioTecnicoDiaLightAsync(string keyword = "")
        {
          Response<IEnumerable<HorarioTecnicoDiaLightDTO>> result = await _HorarioTecnicoDiaService.GetHorarioTecnicoDiaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHorarioTecnicoDiaPaginatedAsync(HorarioTecnicoDiaTableFilter filter)
        {
            PaginatedResponse<HorarioTecnicoDiaTableDTO> result = await _HorarioTecnicoDiaService.GetHorarioTecnicoDiaPaginatedAsync(filter);
            return Ok(result);
        }

        // All HorarioTecnicoDias (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHorarioTecnicoDiaAsync([FromBody] HorarioTecnicoDiaAllFilter filter)
        {
          try
          {
            Response<IEnumerable<HorarioTecnicoDiaTableDTO>> result = await _HorarioTecnicoDiaService.GetAllHorarioTecnicoDiaAsync(filter);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHorarioTecnicoDiaAsync(Guid id)
        {
            Response<HorarioTecnicoDiaDTO> result = await _HorarioTecnicoDiaService.GetHorarioTecnicoDiaAsync(id);
            return Ok(result);
        }

        // Multiple by HorarioTecnicoId
        [Authorize(Roles = "client")]
        [HttpGet("horarioTecnico/{horarioTecnicoId}")]
        public async Task<IActionResult> GetHorarioTecnicoDiaByHorarioTecnicoIdAsync(Guid horarioTecnicoId)
        {
          Response<IEnumerable<HorarioTecnicoDiaDTO>> result = await _HorarioTecnicoDiaService.GetHorarioTecnicoDiaByHorarioTecnicoIdAsync(horarioTecnicoId);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHorarioTecnicoDiaAsync(CreateHorarioTecnicoDiaRequest request)
        {
            try
            {
                Response<Guid> result = await _HorarioTecnicoDiaService.CreateHorarioTecnicoDiaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update
        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHorarioTecnicoDiaAsync(UpdateHorarioTecnicoDiaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _HorarioTecnicoDiaService.UpdateHorarioTecnicoDiaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete
        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorarioTecnicoDiaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HorarioTecnicoDiaService.DeleteHorarioTecnicoDiaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleHorarioTecnicoDiaAsync([FromBody] DeleteMultipleHorarioTecnicoDiaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _HorarioTecnicoDiaService.DeleteMultipleHorarioTecnicoDiaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
