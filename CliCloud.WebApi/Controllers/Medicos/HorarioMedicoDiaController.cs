using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class HorarioMedicoDiaController(IHorarioMedicoDiaService HorarioMedicoDiaService) : ControllerBase
    {
        private readonly IHorarioMedicoDiaService _HorarioMedicoDiaService = HorarioMedicoDiaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHorarioMedicoDiaAsync(string keyword = "")
        {
            Response<IEnumerable<HorarioMedicoDiaDTO>> result = await _HorarioMedicoDiaService.GetHorarioMedicoDiaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHorarioMedicoDiaLightAsync(string keyword = "")
        {
          Response<IEnumerable<HorarioMedicoDiaLightDTO>> result = await _HorarioMedicoDiaService.GetHorarioMedicoDiaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHorarioMedicoDiaPaginatedAsync(HorarioMedicoDiaTableFilter filter)
        {
            PaginatedResponse<HorarioMedicoDiaTableDTO> result = await _HorarioMedicoDiaService.GetHorarioMedicoDiaPaginatedAsync(filter);
            return Ok(result);
        }

        // All HorarioMedicoDias (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHorarioMedicoDiaAsync([FromBody] HorarioMedicoDiaAllFilter filter)
        {
          try
          {
            Response<IEnumerable<HorarioMedicoDiaTableDTO>> result = await _HorarioMedicoDiaService.GetAllHorarioMedicoDiaAsync(filter);
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
        public async Task<IActionResult> GetHorarioMedicoDiaAsync(Guid id)
        {
            Response<HorarioMedicoDiaDTO> result = await _HorarioMedicoDiaService.GetHorarioMedicoDiaAsync(id);
            return Ok(result);
        }

        // Multiple by HorarioMedicoId
        [Authorize(Roles = "client")]
        [HttpGet("horarioMedico/{horarioMedicoId}")]
        public async Task<IActionResult> GetHorarioMedicoDiaByHorarioMedicoIdAsync(Guid horarioMedicoId)
        {
          Response<IEnumerable<HorarioMedicoDiaDTO>> result = await _HorarioMedicoDiaService.GetHorarioMedicoDiaByHorarioMedicoIdAsync(horarioMedicoId);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHorarioMedicoDiaAsync(CreateHorarioMedicoDiaRequest request)
        {
            try
            {
                Response<Guid> result = await _HorarioMedicoDiaService.CreateHorarioMedicoDiaAsync(request);
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
        public async Task<IActionResult> UpdateHorarioMedicoDiaAsync(UpdateHorarioMedicoDiaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _HorarioMedicoDiaService.UpdateHorarioMedicoDiaAsync(request, id);
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
        public async Task<IActionResult> DeleteHorarioMedicoDiaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HorarioMedicoDiaService.DeleteHorarioMedicoDiaAsync(id);
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
        public async Task<IActionResult> DeleteMultipleHorarioMedicoDiaAsync([FromBody] DeleteMultipleHorarioMedicoDiaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _HorarioMedicoDiaService.DeleteMultipleHorarioMedicoDiaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
