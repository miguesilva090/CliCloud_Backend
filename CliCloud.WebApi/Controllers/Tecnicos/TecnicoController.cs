using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tecnicos.TecnicoService;
using CliCloud.Application.Services.Tecnicos.TecnicoService.DTOs;
using CliCloud.Application.Services.Tecnicos.TecnicoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tecnicos
{
    [Route("client/tecnicos/[controller]")]
    [ApiController]
    public class TecnicoController(ITecnicoService TecnicoService) : ControllerBase
    {
        private readonly ITecnicoService _TecnicoService = TecnicoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTecnicoAsync(string keyword = "")
        {
            Response<IEnumerable<TecnicoDTO>> result = await _TecnicoService.GetTecnicoAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTecnicoLightAsync(string keyword = "")
        {
          Response<IEnumerable<TecnicoLightDTO>> result = await _TecnicoService.GetTecnicoLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTecnicoPaginatedAsync(TecnicoTableFilter filter)
        {
            PaginatedResponse<TecnicoTableDTO> result = await _TecnicoService.GetTecnicoPaginatedAsync(filter);
            return Ok(result);
        }

        // All Tecnicos (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTecnicoAsync([FromBody] TecnicoAllFilter filter)
        {
          try
          {
            Response<IEnumerable<TecnicoTableDTO>> result = await _TecnicoService.GetAllTecnicoAsync(filter);
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
        public async Task<IActionResult> GetTecnicoAsync(Guid id)
        {
            Response<TecnicoDTO> result = await _TecnicoService.GetTecnicoAsync(id);
            return Ok(result);
        }

        // Single by NContrib 
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetTecnicoByNContribAsync(string ncontrib)
        {
          Response<TecnicoDTO> result = await _TecnicoService.GetTecnicoByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Multiple by Nome 
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetTecnicoByNameAsync(string nome)
        {
          Response<IEnumerable<TecnicoDTO>> result = await _TecnicoService.GetTecnicoByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTecnicoAsync(CreateTecnicoRequest request)
        {
            try
            {
                Response<Guid> result = await _TecnicoService.CreateTecnicoAsync(request);
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
        public async Task<IActionResult> UpdateTecnicoAsync(UpdateTecnicoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TecnicoService.UpdateTecnicoAsync(request, id);
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
        public async Task<IActionResult> DeleteTecnicoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TecnicoService.DeleteTecnicoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTecnicoAsync([FromBody] DeleteMultipleTecnicoRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TecnicoService.DeleteMultipleTecnicoAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
