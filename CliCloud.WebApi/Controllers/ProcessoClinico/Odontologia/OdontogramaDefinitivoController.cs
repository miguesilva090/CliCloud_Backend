using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.Odontologia.OdontogramaDefinitivoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers
{
    [Route("client/processo-clinico/odontologia/[controller]")]
    [ApiController]
    public class OdontogramaDefinitivoController : ControllerBase
    {
        private readonly IOdontogramaDefinitivoService _odontogramaDefinitivoService;

        public OdontogramaDefinitivoController(IOdontogramaDefinitivoService odontogramaDefinitivoService)
        {
            _odontogramaDefinitivoService = odontogramaDefinitivoService;
        }

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetOdontogramaDefinitivoAsync(string keyword = "")
        {
            Response<IEnumerable<OdontogramaDefinitivoDTO>> result =
                await _odontogramaDefinitivoService.GetOdontogramaDefinitivoAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetOdontogramaDefinitivoPaginatedAsync(OdontogramaDefinitivoTableFilter filter)
        {
            PaginatedResponse<OdontogramaDefinitivoDTO> result =
                await _odontogramaDefinitivoService.GetOdontogramaDefinitivoPaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOdontogramaDefinitivoAsync(Guid id)
        {
            Response<OdontogramaDefinitivoDTO> result =
                await _odontogramaDefinitivoService.GetOdontogramaDefinitivoAsync(id);
            return Ok(result);
        }

        // get by Utente + Consulta (odontograma da consulta)
        [Authorize(Roles = "client")]
        [HttpGet("utente/{utenteId}/consulta/{consultaId}")]
        public async Task<IActionResult> GetByUtenteConsultaAsync(Guid utenteId, Guid consultaId)
        {
            Response<IEnumerable<OdontogramaDefinitivoDTO>> result =
                await _odontogramaDefinitivoService.GetByUtenteConsultaAsync(utenteId, consultaId);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateOdontogramaDefinitivoAsync(CreateOdontogramaDefinitivoRequest request)
        {
            try
            {
                Response<Guid> result =
                    await _odontogramaDefinitivoService.CreateOdontogramaDefinitivoAsync(request);
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
        public async Task<IActionResult> UpdateOdontogramaDefinitivoAsync(UpdateOdontogramaDefinitivoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result =
                    await _odontogramaDefinitivoService.UpdateOdontogramaDefinitivoAsync(request, id);
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
        public async Task<IActionResult> DeleteOdontogramaDefinitivoAsync(Guid id)
        {
            try
            {
                Response<Guid> response =
                    await _odontogramaDefinitivoService.DeleteOdontogramaDefinitivoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
