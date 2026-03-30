using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class LocalTratamentoController(ILocalTratamentoService LocalTratamentoService) : ControllerBase
    {
        private readonly ILocalTratamentoService _localTratamentoService = LocalTratamentoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetLocalTratamentoAsync(string keyword = "")
        {
            Response<IEnumerable<LocalTratamentoDTO>> result = await _localTratamentoService.GetLocalTratamentoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetLocalTratamentoLightAsync(string keyword = "")
        {
            Response<IEnumerable<LocalTratamentoLightDTO>> result = await _localTratamentoService.GetLocalTratamentoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetLocalTratamentoPaginatedAsync(LocalTratamentoTableFilter filter)
        {
            PaginatedResponse<LocalTratamentoTableDTO> result = await _localTratamentoService.GetLocalTratamentoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllLocalTratamentoAsync([FromBody] LocalTratamentoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<LocalTratamentoTableDTO>> result = await _localTratamentoService.GetAllLocalTratamentoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocalTratamentoAsync(Guid id)
        {
            Response<LocalTratamentoDTO> result = await _localTratamentoService.GetLocalTratamentoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateLocalTratamentoAsync(CreateLocalTratamentoRequest request)
        {
            try
            {
                Response<Guid> result = await _localTratamentoService.CreateLocalTratamentoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocalTratamentoAsync(UpdateLocalTratamentoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _localTratamentoService.UpdateLocalTratamentoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalTratamentoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _localTratamentoService.DeleteLocalTratamentoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleLocalTratamentoAsync([FromBody] DeleteMultipleLocalTratamentoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _localTratamentoService.DeleteMultipleLocalTratamentoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
