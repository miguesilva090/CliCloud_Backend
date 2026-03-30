using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.DTOs;
using CliCloud.Application.Services.Antecedentes.AntecedentesPessoaisService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloudWebApi.Controllers.Antecedentes
{
    [Route("client/antecedentes/[controller]")]
    [ApiController]
    public class AntecedentesPessoaisController(IAntecedentesPessoaisService antecedentesPessoaisService) : ControllerBase
    {
        private readonly IAntecedentesPessoaisService _antecedentesPessoaisService = antecedentesPessoaisService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAntecedentesPessoaisAsync(string keyword = "")
        {
            Response<IEnumerable<AntecedentesPessoaisDTO>> result = await _antecedentesPessoaisService.GetAntecedentesPessoaisAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetAntecedentesPessoaisLightAsync(string keyword = "") 
        {
            Response<IEnumerable<AntecedentesPessoaisLightDTO>> result = await _antecedentesPessoaisService.GetAntecedentesPessoaisLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetAntecedentesPessoaisPaginatedAsync(AntecedentesPessoaisTableFilter filter)
        {
            PaginatedResponse<AntecedentesPessoaisTableDTO> result = await _antecedentesPessoaisService.GetAntecedentesPessoaisPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAntecedentesPessoaisAsync([FromBody] AntecedentesPessoaisAllFilter filter)
        {
            Response<IEnumerable<AntecedentesPessoaisTableDTO>> result = await _antecedentesPessoaisService.GetAllAntecedentesPessoaisAsync(filter);
            return Ok(result);
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAntecedentesPessoaisAsync(Guid id)
        {
            Response<AntecedentesPessoaisDTO> result = await _antecedentesPessoaisService.GetAntecedentesPessoaisAsync(id);
            return Ok(result);
        }

        // single by Nome Doenca 
        [Authorize(Roles = "client")]
        [HttpGet("nome-doenca/{nomeDoenca}")]
        public async Task<IActionResult> GetAntecedentesPessoaisByNomeDoencaAsync(string nomeDoenca)
        {
            Response<AntecedentesPessoaisDTO> result = await _antecedentesPessoaisService.GetAntecedentesPessoaisByNomeDoencaAsync(nomeDoenca);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAntecedentesPessoaisAsync(CreateAntecedentesPessoaisRequest request)
        {
            try
            {
                Response<Guid> result = await _antecedentesPessoaisService.CreateAntecedentesPessoaisAsync(request);
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
        public async Task<IActionResult> UpdateAntecedentesPessoaisAsync(UpdateAntecedentesPessoaisRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _antecedentesPessoaisService.UpdateAntecedentesPessoaisAsync(request, id);
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
        public async Task<IActionResult> DeleteAntecedentesPessoaisAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _antecedentesPessoaisService.DeleteAntecedentesPessoaisAsync(id);
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
        public async Task<IActionResult> DeleteMultipleAntecedentesPessoaisAsync([FromBody] DeleteMultipleAntecedentesPessoaisRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> response = await _antecedentesPessoaisService.DeleteMultipleAntecedentesPessoaisAsync(request.Ids);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
