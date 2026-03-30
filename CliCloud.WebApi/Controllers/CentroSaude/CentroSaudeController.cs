using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs;
using CliCloud.Application.Services.CentroSaude.CentroSaudeService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.CentroSaude
{
    [Route("client/centro-saude/[controller]")]
    [ApiController]
    public class CentroSaudeController(ICentroSaudeService CentroSaudeService) : ControllerBase
    {
        private readonly ICentroSaudeService _CentroSaudeService = CentroSaudeService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetCentroSaudeAsync(string keyword = "")
        {
            Response<IEnumerable<CentroSaudeDTO>> result = await _CentroSaudeService.GetCentroSaudeAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetCentroSaudeLightAsync(string keyword = "")
        {
          Response<IEnumerable<CentroSaudeLightDTO>> result = await _CentroSaudeService.GetCentroSaudeLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetCentroSaudePaginatedAsync(CentroSaudeTableFilter filter)
        {
            PaginatedResponse<CentroSaudeTableDTO> result = await _CentroSaudeService.GetCentroSaudePaginatedAsync(filter);
            return Ok(result);
        }

        //All CentroSaude (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCentroSaudeAsync([FromBody] CentroSaudeAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<CentroSaudeTableDTO>> result = await _CentroSaudeService.GetAllCentroSaudeAsync(filter ?? new CentroSaudeAllFilter());
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
        public async Task<IActionResult> GetCentroSaudeAsync(Guid id)
        {
            Response<CentroSaudeDTO> result = await _CentroSaudeService.GetCentroSaudeAsync(id);
            return Ok(result);
        }

        //Single by NContrib
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetCentroSaudeByNContribAsync(string ncontrib)
        {
          Response<CentroSaudeDTO> result = await _CentroSaudeService.GetCentroSaudeByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Get multiple by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetCentroSaudeByNameAsync(string nome)
        {
          Response<IEnumerable<CentroSaudeDTO>> result = await _CentroSaudeService.GetCentroSaudeByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateCentroSaudeAsync(CreateCentroSaudeRequest request)
        {
            try
            {
                Response<Guid> result = await _CentroSaudeService.CreateCentroSaudeAsync(request);
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
        public async Task<IActionResult> UpdateCentroSaudeAsync(UpdateCentroSaudeRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _CentroSaudeService.UpdateCentroSaudeAsync(request, id);
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
        public async Task<IActionResult> DeleteCentroSaudeAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _CentroSaudeService.DeleteCentroSaudeAsync(id);
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
        public async Task<IActionResult> DeleteMultipleCentroSaudeAsync([FromBody] DeleteMultipleCentroSaudeRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _CentroSaudeService.DeleteMultipleCentroSaudeAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
