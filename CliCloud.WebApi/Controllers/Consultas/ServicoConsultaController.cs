using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Consultas.ServicoConsultaService;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Consultas
{
    [Route("client/consultas/[controller]")]
    [ApiController]
    public class ServicoConsultaController(IServicoConsultaService ServicoConsultaService) : ControllerBase
    {
        private readonly IServicoConsultaService _ServicoConsultaService = ServicoConsultaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetServicoConsultaAsync(string keyword = "")
        {
            Response<IEnumerable<ServicoConsultaDTO>> result = await _ServicoConsultaService.GetServicoConsultaAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetServicoConsultaLightAsync(string keyword = "")
        {
          Response<IEnumerable<ServicoConsultaLightDTO>> result = await _ServicoConsultaService.GetServicoConsultaLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetServicoConsultaPaginatedAsync(ServicoConsultaTableFilter filter)
        {
            PaginatedResponse<ServicoConsultaTableDTO> result = await _ServicoConsultaService.GetServicoConsultaPaginatedAsync(filter);
            return Ok(result);
        }

        // All ServicoConsultas (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllServicoConsultaAsync([FromBody] ServicoConsultaAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<ServicoConsultaTableDTO>> result = await _ServicoConsultaService.GetAllServicoConsultaAsync(filter ?? new ServicoConsultaAllFilter());
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
        public async Task<IActionResult> GetServicoConsultaAsync(Guid id)
        {
            Response<ServicoConsultaDTO> result = await _ServicoConsultaService.GetServicoConsultaAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateServicoConsultaAsync(CreateServicoConsultaRequest request)
        {
            try
            {
                Response<Guid> result = await _ServicoConsultaService.CreateServicoConsultaAsync(request);
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
        public async Task<IActionResult> UpdateServicoConsultaAsync(UpdateServicoConsultaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ServicoConsultaService.UpdateServicoConsultaAsync(request, id);
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
        public async Task<IActionResult> DeleteServicoConsultaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ServicoConsultaService.DeleteServicoConsultaAsync(id);
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
        public async Task<IActionResult> DeleteMultipleServicoConsultaAsync([FromBody] DeleteMultipleServicoConsultaRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ServicoConsultaService.DeleteMultipleServicoConsultaAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
