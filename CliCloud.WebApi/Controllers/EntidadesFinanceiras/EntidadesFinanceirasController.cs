using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.EntidadesFinanceiras
{
    [Route("client/entidades-financeiras/[controller]")]
    [ApiController]
    public class EntidadesFinanceirasController(IEntidadeFinanceiraService EntidadeFinanceiraService) : ControllerBase
    {
        private readonly IEntidadeFinanceiraService _EntidadeFinanceiraService = EntidadeFinanceiraService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetEntidadesFinanceirasAsync(string keyword = "")
        {
            Response<IEnumerable<EntidadeFinanceiraDTO>> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetEntidadeFinanceiraLightAsync(string keyword = "")
        {
          Response<IEnumerable<EntidadeFinanceiraLightDTO>> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetEntidadeFinanceiraPaginatedAsync(EntidadeFinanceiraTableFilter filter)
        {
            PaginatedResponse<EntidadeFinanceiraTableDTO> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraPaginatedAsync(filter);
            return Ok(result);
        }

        // All EntidadesFinanceiras (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllEntidadeFinanceiraAsync([FromBody] EntidadeFinanceiraAllFilter? filter = null)
        {
          try
          {
            Response<IEnumerable<EntidadeFinanceiraTableDTO>> result = await _EntidadeFinanceiraService.GetAllEntidadeFinanceiraAsync(filter ?? new EntidadeFinanceiraAllFilter());
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
        public async Task<IActionResult> GetEntidadeFinanceiraAsync(Guid id)
        {
            Response<EntidadeFinanceiraDTO> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraAsync(id);
            return Ok(result);
        }

        // Single by NContrib
        [Authorize(Roles = "client")]
        [HttpGet("ncontrib/{ncontrib}")]
        public async Task<IActionResult> GetEntidadeFinanceiraByNContribAsync(string ncontrib)
        {
          Response<EntidadeFinanceiraDTO> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraByNContribAsync(ncontrib);
          return Ok(result);
        }

        // Get multiple by Nome
        [Authorize(Roles = "client")]
        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> GetEntidadeFinanceiraByNameAsync(string nome)
        {
          Response<IEnumerable<EntidadeFinanceiraDTO>> result = await _EntidadeFinanceiraService.GetEntidadeFinanceiraByNameAsync(nome);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateEntidadeFinanceiraAsync(CreateEntidadeFinanceiraRequest request)
        {
            try
            {
                Response<Guid> result = await _EntidadeFinanceiraService.CreateEntidadeFinanceiraAsync(request);
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
        public async Task<IActionResult> UpdateEntidadeFinanceiraAsync(UpdateEntidadeFinanceiraRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _EntidadeFinanceiraService.UpdateEntidadeFinanceiraAsync(request, id);
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
        public async Task<IActionResult> DeleteEntidadeFinanceiraAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _EntidadeFinanceiraService.DeleteEntidadeFinanceiraAsync(id);
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
        public async Task<IActionResult> DeleteMultipleEntidadeFinanceiraAsync([FromBody] DeleteMultipleEntidadeFinanceiraRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _EntidadeFinanceiraService.DeleteMultipleEntidadeFinanceiraAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}
