using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs;
using CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class EstadoListaEsperaController(IEstadoListaEsperaService estadoListaEsperaService) : ControllerBase
    {
        private readonly IEstadoListaEsperaService _estadoListaEsperaService = estadoListaEsperaService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEstadoListaEsperaAsync(string keyword = "")
        {
            Response<IEnumerable<EstadoListaEsperaDTO>> result = await _estadoListaEsperaService.GetEstadoListaEsperaAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEstadoListaEsperaLightAsync(string keyword = "")
        {
            Response<IEnumerable<EstadoListaEsperaLightDTO>> result = await _estadoListaEsperaService.GetEstadoListaEsperaLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEstadoListaEsperaPaginatedAsync(EstadoListaEsperaTableFilter filter)
        {
            PaginatedResponse<EstadoListaEsperaTableDTO> result = await _estadoListaEsperaService.GetEstadoListaEsperaPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEstadoListaEsperaAsync([FromBody] EstadoListaEsperaAllFilter filter)
        {
            try
            {
                Response<IEnumerable<EstadoListaEsperaTableDTO>> result = await _estadoListaEsperaService.GetAllEstadoListaEsperaAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstadoListaEsperaAsync(Guid id)
        {
            Response<EstadoListaEsperaDTO> result = await _estadoListaEsperaService.GetEstadoListaEsperaAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEstadoListaEsperaAsync(CreateEstadoListaEsperaRequest request)
        {
            try
            {
                Response<Guid> result = await _estadoListaEsperaService.CreateEstadoListaEsperaAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstadoListaEsperaAsync(UpdateEstadoListaEsperaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _estadoListaEsperaService.UpdateEstadoListaEsperaAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoListaEsperaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _estadoListaEsperaService.DeleteEstadoListaEsperaAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleEstadoListaEsperaAsync([FromBody] DeleteMultipleEstadoListaEsperaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _estadoListaEsperaService.DeleteMultipleEstadoListaEsperaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
