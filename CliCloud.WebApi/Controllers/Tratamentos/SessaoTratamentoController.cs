using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class SessaoTratamentoController(ISessaoTratamentoService SessaoTratamentoService) : ControllerBase
    {
        private readonly ISessaoTratamentoService _SessaoTratamentoService = SessaoTratamentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetSessaoTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<SessaoTratamentoDTO>> result = await _SessaoTratamentoService.GetSessaoTratamentoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetSessaoTratamentoLightAsync(string keyword = "")
        {
          Response<IEnumerable<SessaoTratamentoLightDTO>> result = await _SessaoTratamentoService.GetSessaoTratamentoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetSessaoTratamentoPaginatedAsync(SessaoTratamentoTableFilter filter)
        {
            PaginatedResponse<SessaoTratamentoTableDTO> result = await _SessaoTratamentoService.GetSessaoTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        // All SessaoTratamentos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllSessaoTratamentoAsync([FromBody] SessaoTratamentoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<SessaoTratamentoTableDTO>> result = await _SessaoTratamentoService.GetAllSessaoTratamentoAsync(filter);
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
        public async Task<IActionResult> GetSessaoTratamentoAsync(Guid id)
        {
            Response<SessaoTratamentoDTO> result = await _SessaoTratamentoService.GetSessaoTratamentoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateSessaoTratamentoAsync(CreateSessaoTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _SessaoTratamentoService.CreateSessaoTratamentoAsync(request);
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
        public async Task<IActionResult> UpdateSessaoTratamentoAsync(UpdateSessaoTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _SessaoTratamentoService.UpdateSessaoTratamentoAsync(request, id);
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
        public async Task<IActionResult> DeleteSessaoTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _SessaoTratamentoService.DeleteSessaoTratamentoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleSessaoTratamentoAsync([FromBody] DeleteMultipleSessaoTratamentoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _SessaoTratamentoService.DeleteMultipleSessaoTratamentoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
