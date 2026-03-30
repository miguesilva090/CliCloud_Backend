using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.TratamentoService;
using CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class TratamentoController(ITratamentoService TratamentoService) : ControllerBase
    {
        private readonly ITratamentoService _TratamentoService = TratamentoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<TratamentoDTO>> result = await _TratamentoService.GetTratamentoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTratamentoLightAsync(string keyword = "")
        {
          Response<IEnumerable<TratamentoLightDTO>> result = await _TratamentoService.GetTratamentoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTratamentoPaginatedAsync(TratamentoTableFilter filter)
        {
            PaginatedResponse<TratamentoTableDTO> result = await _TratamentoService.GetTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Tratamentos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTratamentoAsync([FromBody] TratamentoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<TratamentoTableDTO>> result = await _TratamentoService.GetAllTratamentoAsync(filter);
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
        public async Task<IActionResult> GetTratamentoAsync(Guid id)
        {
            Response<TratamentoDTO> result = await _TratamentoService.GetTratamentoAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTratamentoAsync(CreateTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _TratamentoService.CreateTratamentoAsync(request);
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
        public async Task<IActionResult> UpdateTratamentoAsync(UpdateTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TratamentoService.UpdateTratamentoAsync(request, id);
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
        public async Task<IActionResult> DeleteTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TratamentoService.DeleteTratamentoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // update alta (alta/retirar alta)
        [Authorize(Roles = "client")]
        [HttpPost("alta")]
        public async Task<IActionResult> UpdateTratamentoAltaAsync([FromBody] UpdateTratamentoAltaRequest request)
        {
            try
            {
                Response<Guid> result = await _TratamentoService.UpdateTratamentoAltaAsync(request.Id, request.Alta);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete Multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleTratamentoAsync([FromBody] DeleteMultipleTratamentoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TratamentoService.DeleteMultipleTratamentoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
