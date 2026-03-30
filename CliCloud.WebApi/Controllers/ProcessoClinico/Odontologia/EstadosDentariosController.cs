using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/odontologia/[controller]")]
    [ApiController]
    public class EstadosDentariosController : ControllerBase
    {
        private readonly IEstadosDentariosService _EstadosDentariosService;

        public EstadosDentariosController(IEstadosDentariosService EstadosDentariosService)
        {
            _EstadosDentariosService = EstadosDentariosService;
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEstadosDentariosAsync(string keyword = "")
        {
            Response<IEnumerable<EstadosDentariosDTO>> result = await _EstadosDentariosService.GetEstadosDentariosAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEstadosDentariosPaginatedAsync(EstadosDentariosTableFilter filter)
        {
            PaginatedResponse<EstadosDentariosDTO> result = await _EstadosDentariosService.GetEstadosDentariosPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstadosDentariosAsync(Guid id)
        {
            Response<EstadosDentariosDTO> result = await _EstadosDentariosService.GetEstadosDentariosAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEstadosDentariosAsync(CreateEstadosDentariosRequest request)
        {
            try
            {
                Response<Guid> result = await _EstadosDentariosService.CreateEstadosDentariosAsync(request);
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
        public async Task<IActionResult> UpdateEstadosDentariosAsync(UpdateEstadosDentariosRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _EstadosDentariosService.UpdateEstadosDentariosAsync(request, id);
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
        public async Task<IActionResult> DeleteEstadosDentariosAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EstadosDentariosService.DeleteEstadosDentariosAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
