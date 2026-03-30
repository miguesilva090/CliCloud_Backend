using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Antecedentes
{
    [Route("client/antecedentes/[controller]")]
    [ApiController]
    public class AntecedentesFamiliaresUtenteController(IAntecedentesFamiliaresUtenteService antecedentesFamiliaresUtenteService) : ControllerBase
    {
        private readonly IAntecedentesFamiliaresUtenteService _antecedentesFamiliaresUtenteService = antecedentesFamiliaresUtenteService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAntecedentesFamiliaresUtenteAsync(string keyword = "")
        {
            Response<IEnumerable<AntecedentesFamiliaresUtenteDTO>> result = await _antecedentesFamiliaresUtenteService.GetAntecedentesFamiliaresUtenteAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAntecedentesFamiliaresUtenteLightAsync(string keyword = "")
        {
            Response<IEnumerable<AntecedentesFamiliaresUtenteLightDTO>> result = await _antecedentesFamiliaresUtenteService.GetAntecedentesFamiliaresUtenteLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAntecedentesFamiliaresUtentePaginatedAsync(AntecedentesFamiliaresUtenteTableFilter filter)
        {
            PaginatedResponse<AntecedentesFamiliaresUtenteTableDTO> result = await _antecedentesFamiliaresUtenteService.GetAntecedentesFamiliaresUtentePaginatedAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAntecedentesFamiliaresUtenteAsync(Guid id)
        {
            Response<AntecedentesFamiliaresUtenteDTO> result = await _antecedentesFamiliaresUtenteService.GetAntecedentesFamiliaresUtenteAsync(id);
            return Ok(result);
        }

        // single by NomeDoenca
        [Authorize(Roles = "client")]
        [HttpGet("nome-doenca/{nomeDoenca}")]
        public async Task<IActionResult> GetAntecedentesFamiliaresUtenteByNomeDoencaAsync(string nomeDoenca)
        {
            Response<AntecedentesFamiliaresUtenteDTO> result = await _antecedentesFamiliaresUtenteService.GetAntecedentesFamiliaresUtenteByNomeDoencaAsync(nomeDoenca);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAntecedentesFamiliaresUtenteAsync(CreateAntecedentesFamiliaresUtenteRequest request)
        {
            try
            {
                Response<Guid> result = await _antecedentesFamiliaresUtenteService.CreateAntecedentesFamiliaresUtenteAsync(request);
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
        public async Task<IActionResult> UpdateAntecedentesFamiliaresUtenteAsync(UpdateAntecedentesFamiliaresUtenteRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _antecedentesFamiliaresUtenteService.UpdateAntecedentesFamiliaresUtenteAsync(request, id);
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
        public async Task<IActionResult> DeleteAntecedentesFamiliaresUtenteAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _antecedentesFamiliaresUtenteService.DeleteAntecedentesFamiliaresUtenteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // delete multiple 
        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAntecedentesFamiliaresUtenteAsync([FromBody] DeleteMultipleAntecedentesFamiliaresUtenteRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _antecedentesFamiliaresUtenteService.DeleteMultipleAntecedentesFamiliaresUtenteAsync(request.Ids);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
