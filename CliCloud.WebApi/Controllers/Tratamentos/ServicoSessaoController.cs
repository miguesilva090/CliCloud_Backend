using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class ServicoSessaoController(IServicoSessaoService ServicoSessaoService) : ControllerBase
    {
        private readonly IServicoSessaoService _ServicoSessaoService = ServicoSessaoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetServicoSessaoAsync(string keyword = "")
        {
            Response<IEnumerable<ServicoSessaoDTO>> result = await _ServicoSessaoService.GetServicoSessaoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetServicoSessaoLightAsync(string keyword = "")
        {
          Response<IEnumerable<ServicoSessaoLightDTO>> result = await _ServicoSessaoService.GetServicoSessaoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetServicoSessaoPaginatedAsync(ServicoSessaoTableFilter filter)
        {
            PaginatedResponse<ServicoSessaoTableDTO> result = await _ServicoSessaoService.GetServicoSessaoPaginatedAsync(filter);
            return Ok(result);
        }

        // All ServicoSessoes (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllServicoSessaoAsync([FromBody] ServicoSessaoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<ServicoSessaoTableDTO>> result = await _ServicoSessaoService.GetAllServicoSessaoAsync(filter);
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
        public async Task<IActionResult> GetServicoSessaoAsync(Guid id)
        {
            Response<ServicoSessaoDTO> result = await _ServicoSessaoService.GetServicoSessaoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateServicoSessaoAsync(CreateServicoSessaoRequest request)
        {
            try
            {
                Response<Guid> result = await _ServicoSessaoService.CreateServicoSessaoAsync(request);
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
        public async Task<IActionResult> UpdateServicoSessaoAsync(UpdateServicoSessaoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ServicoSessaoService.UpdateServicoSessaoAsync(request, id);
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
        public async Task<IActionResult> DeleteServicoSessaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ServicoSessaoService.DeleteServicoSessaoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleServicoSessaoAsync([FromBody] DeleteMultipleServicoSessaoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ServicoSessaoService.DeleteMultipleServicoSessaoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
