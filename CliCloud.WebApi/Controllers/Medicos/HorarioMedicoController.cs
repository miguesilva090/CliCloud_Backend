using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Medicos.HorarioMedicoService;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Medicos
{
    [Route("client/medicos/[controller]")]
    [ApiController]
    public class HorarioMedicoController(IHorarioMedicoService HorarioMedicoService) : ControllerBase
    {
        private readonly IHorarioMedicoService _HorarioMedicoService = HorarioMedicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHorarioMedicoAsync(string keyword = "")
        {
            Response<IEnumerable<HorarioMedicoDTO>> result = await _HorarioMedicoService.GetHorarioMedicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHorarioMedicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<HorarioMedicoLightDTO>> result = await _HorarioMedicoService.GetHorarioMedicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHorarioMedicoPaginatedAsync(HorarioMedicoTableFilter filter)
        {
            PaginatedResponse<HorarioMedicoTableDTO> result = await _HorarioMedicoService.GetHorarioMedicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All HorariosMedicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHorarioMedicoAsync([FromBody] HorarioMedicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<HorarioMedicoTableDTO>> result = await _HorarioMedicoService.GetAllHorarioMedicoAsync(filter);
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
        public async Task<IActionResult> GetHorarioMedicoAsync(Guid id)
        {
            Response<HorarioMedicoDTO> result = await _HorarioMedicoService.GetHorarioMedicoAsync(id);
            return Ok(result);
        }

        // Multiple by MedicoId
        [Authorize(Roles = "client")]
        [HttpGet("medico/{medicoId}")]
        public async Task<IActionResult> GetHorarioMedicoByMedicoIdAsync(Guid medicoId)
        {
          Response<IEnumerable<HorarioMedicoDTO>> result = await _HorarioMedicoService.GetHorarioMedicoByMedicoIdAsync(medicoId);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHorarioMedicoAsync(CreateHorarioMedicoRequest request)
        {
            try
            {
                Response<Guid> result = await _HorarioMedicoService.CreateHorarioMedicoAsync(request);
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
        public async Task<IActionResult> UpdateHorarioMedicoAsync(UpdateHorarioMedicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _HorarioMedicoService.UpdateHorarioMedicoAsync(request, id);
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
        public async Task<IActionResult> DeleteHorarioMedicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HorarioMedicoService.DeleteHorarioMedicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleHorarioMedicoAsync([FromBody] DeleteMultipleHorarioMedicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _HorarioMedicoService.DeleteMultipleHorarioMedicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
