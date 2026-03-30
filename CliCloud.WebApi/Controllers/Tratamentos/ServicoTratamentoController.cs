using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class ServicoTratamentoController(IServicoTratamentoService ServicoTratamentoService) : ControllerBase
    {
        private readonly IServicoTratamentoService _ServicoTratamentoService = ServicoTratamentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetServicoTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<ServicoTratamentoDTO>> result = await _ServicoTratamentoService.GetServicoTratamentoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetServicoTratamentoLightAsync(string keyword = "")
        {
          Response<IEnumerable<ServicoTratamentoLightDTO>> result = await _ServicoTratamentoService.GetServicoTratamentoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetServicoTratamentoPaginatedAsync(ServicoTratamentoTableFilter filter)
        {
            PaginatedResponse<ServicoTratamentoTableDTO> result = await _ServicoTratamentoService.GetServicoTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        // All ServicoTratamentos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllServicoTratamentoAsync([FromBody] ServicoTratamentoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<ServicoTratamentoTableDTO>> result = await _ServicoTratamentoService.GetAllServicoTratamentoAsync(filter);
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
        public async Task<IActionResult> GetServicoTratamentoAsync(Guid id)
        {
            Response<ServicoTratamentoDTO> result = await _ServicoTratamentoService.GetServicoTratamentoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateServicoTratamentoAsync(CreateServicoTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _ServicoTratamentoService.CreateServicoTratamentoAsync(request);
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
        public async Task<IActionResult> UpdateServicoTratamentoAsync(UpdateServicoTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _ServicoTratamentoService.UpdateServicoTratamentoAsync(request, id);
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
        public async Task<IActionResult> DeleteServicoTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _ServicoTratamentoService.DeleteServicoTratamentoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleServicoTratamentoAsync([FromBody] DeleteMultipleServicoTratamentoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _ServicoTratamentoService.DeleteMultipleServicoTratamentoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
