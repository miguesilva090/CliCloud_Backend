using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Consultas.ConsultaService;
using CliCloud.Application.Services.Consultas.ConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ConsultaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Consultas
{
    [Route("client/consultas/[controller]")]
    [ApiController]
    public class ConsultaController(IConsultaService consultaService) : ControllerBase
    {
        private readonly IConsultaService _consultaService = consultaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetConsultaAsync(string keyword = "")
        {
            Response<IEnumerable<ConsultaDTO>> result = await _consultaService.GetConsultaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetConsultaLightAsync(string keyword = "")
        {
          Response<IEnumerable<ConsultaLightDTO>> result = await _consultaService.GetConsultaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetConsultaPaginatedAsync(ConsultaTableFilter filter)
        {
            PaginatedResponse<ConsultaTableDTO> result = await _consultaService.GetConsultaPaginatedAsync(filter);
            return Ok(result);
        }

        // All Consultas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllConsultaAsync([FromBody] ConsultaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<ConsultaTableDTO>> result = await _consultaService.GetAllConsultaAsync(filter ?? new ConsultaAllFilter());
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
        public async Task<IActionResult> GetConsultaAsync(Guid id)
        {
            Response<ConsultaDTO> result = await _consultaService.GetConsultaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateConsultaAsync(CreateConsultaRequest request)
        {
            try
            {
                Response<Guid> result = await _consultaService.CreateConsultaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // create from ConsultaMarcacao (agenda -> ato clínico)
        [Authorize(Roles = "client")]
        [HttpPost("from-marcacao/{marcacaoId}")]
        public async Task<IActionResult> CreateConsultaFromMarcacaoAsync(Guid marcacaoId)
        {
            try
            {
                Response<Guid> result = await _consultaService.CreateConsultaFromMarcacaoAsync(marcacaoId);
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
        public async Task<IActionResult> UpdateConsultaAsync(UpdateConsultaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _consultaService.UpdateConsultaAsync(request, id);
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
        public async Task<IActionResult> DeleteConsultaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _consultaService.DeleteConsultaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete multiple
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleConsultaAsync([FromBody] DeleteMultipleConsultaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> response = await _consultaService.DeleteMultipleConsultaAsync(request.Ids);
            return Ok(response);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        // finalizar consulta (EmAtendimento -> Concluída)
        [Authorize(Roles = "client")]
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarConsultaAsync(Guid id)
        {
            try
            {
                Response<Guid> result = await _consultaService.FinalizarConsultaAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
