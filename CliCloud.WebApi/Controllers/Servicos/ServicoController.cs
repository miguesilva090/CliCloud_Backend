using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Servicos.ServicoService;
using CliCloud.Application.Services.Servicos.ServicoService.DTOs;
using CliCloud.Application.Services.Servicos.ServicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Servicos
{
    [Route("client/servicos/[controller]")]
    [ApiController]
    public class ServicoController(IServicoService ServicoService) : ControllerBase
    {
        private readonly IServicoService _ServicoService = ServicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetServicoAsync(string keyword = "")
        {
            Response<IEnumerable<ServicoDTO>> result = await _ServicoService.GetServicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetServicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<ServicoLightDTO>> result = await _ServicoService.GetServicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetServicoPaginatedAsync(ServicoTableFilter filter)
        {
            PaginatedResponse<ServicoTableDTO> result = await _ServicoService.GetServicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Servicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllServicoAsync([FromBody] ServicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<ServicoTableDTO>> result = await _ServicoService.GetAllServicoAsync(filter);
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
        public async Task<IActionResult> GetServicoAsync(Guid id)
        {
            Response<ServicoDTO> result = await _ServicoService.GetServicoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateServicoAsync(CreateServicoRequest request)
        {
            try
            {
                Response<Guid> result = await _ServicoService.CreateServicoAsync(request);
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
        public async Task<IActionResult> UpdateServicoAsync(UpdateServicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ServicoService.UpdateServicoAsync(request, id);
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
        public async Task<IActionResult> DeleteServicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ServicoService.DeleteServicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleServicoAsync([FromBody] DeleteMultipleServicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ServicoService.DeleteMultipleServicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
