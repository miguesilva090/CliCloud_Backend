using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tecnicos
{
    [Route("client/tecnicos/[controller]")]
    [ApiController]
    public class HorarioTecnicoController(IHorarioTecnicoService HorarioTecnicoService) : ControllerBase
    {
        private readonly IHorarioTecnicoService _HorarioTecnicoService = HorarioTecnicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetHorarioTecnicoAsync(string keyword = "")
        {
            Response<IEnumerable<HorarioTecnicoDTO>> result = await _HorarioTecnicoService.GetHorarioTecnicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetHorarioTecnicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<HorarioTecnicoLightDTO>> result = await _HorarioTecnicoService.GetHorarioTecnicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetHorarioTecnicoPaginatedAsync(HorarioTecnicoTableFilter filter)
        {
            PaginatedResponse<HorarioTecnicoTableDTO> result = await _HorarioTecnicoService.GetHorarioTecnicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All HorarioTecnicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllHorarioTecnicoAsync([FromBody] HorarioTecnicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<HorarioTecnicoTableDTO>> result = await _HorarioTecnicoService.GetAllHorarioTecnicoAsync(filter);
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
        public async Task<IActionResult> GetHorarioTecnicoAsync(Guid id)
        {
            Response<HorarioTecnicoDTO> result = await _HorarioTecnicoService.GetHorarioTecnicoAsync(id);
            return Ok(result);
        }

        // Multiple by TecnicoId
        [Authorize(Roles = "client")]
        [HttpGet("tecnico/{tecnicoId}")]
        public async Task<IActionResult> GetHorarioTecnicoByTecnicoIdAsync(Guid tecnicoId)
        {
          Response<IEnumerable<HorarioTecnicoDTO>> result = await _HorarioTecnicoService.GetHorarioTecnicoByTecnicoIdAsync(tecnicoId);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateHorarioTecnicoAsync(CreateHorarioTecnicoRequest request)
        {
            try
            {
                Response<Guid> result = await _HorarioTecnicoService.CreateHorarioTecnicoAsync(request);
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
        public async Task<IActionResult> UpdateHorarioTecnicoAsync(UpdateHorarioTecnicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _HorarioTecnicoService.UpdateHorarioTecnicoAsync(request, id);
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
        public async Task<IActionResult> DeleteHorarioTecnicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _HorarioTecnicoService.DeleteHorarioTecnicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleHorarioTecnicoAsync([FromBody] DeleteMultipleHorarioTecnicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _HorarioTecnicoService.DeleteMultipleHorarioTecnicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
