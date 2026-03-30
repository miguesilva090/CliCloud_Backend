using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/odontologia/[controller]")]
    [ApiController]
    public class TiposTratamentoDentarioController : ControllerBase
    {
        private readonly ITiposTratamentoDentarioService _tiposTratamentoDentarioService;

        public TiposTratamentoDentarioController(ITiposTratamentoDentarioService tiposTratamentoDentarioService)
        {
            _tiposTratamentoDentarioService = tiposTratamentoDentarioService;
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTiposTratamentoDentarioAsync(string keyword = "")
        {
            Response<IEnumerable<TiposTratamentoDentarioDTO>> result =
                await _tiposTratamentoDentarioService.GetTiposTratamentoDentarioAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTiposTratamentoDentarioPaginatedAsync(TiposTratamentoDentarioTableFilter filter)
        {
            PaginatedResponse<TiposTratamentoDentarioDTO> result =
                await _tiposTratamentoDentarioService.GetTiposTratamentoDentarioPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTiposTratamentoDentarioAsync(Guid id)
        {
            Response<TiposTratamentoDentarioDTO> result =
                await _tiposTratamentoDentarioService.GetTiposTratamentoDentarioAsync(id);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTiposTratamentoDentarioAsync(CreateTiposTratamentoDentarioRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _tiposTratamentoDentarioService.CreateTiposTratamentoDentarioAsync(request);
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
        public async Task<IActionResult> UpdateTiposTratamentoDentarioAsync(UpdateTiposTratamentoDentarioRequest request, Guid id)
        {
            try
            {
                Response<Guid> result =
                    await _tiposTratamentoDentarioService.UpdateTiposTratamentoDentarioAsync(request, id);
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
        public async Task<IActionResult> DeleteTiposTratamentoDentarioAsync(Guid id)
        {
            try
            {
                Response<Guid> response =
                    await _tiposTratamentoDentarioService.DeleteTiposTratamentoDentarioAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
