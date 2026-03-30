using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs;
using CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.TipoEntidadeFinanceira
{
    [Route("client/tipoEntidadeFinanceira/[controller]")]
    [ApiController]
    public class TipoEntidadeFinanceiraController(ITipoEntidadeFinanceiraService TipoEntidadeFinanceiraService) : ControllerBase
    {
        private readonly ITipoEntidadeFinanceiraService _TipoEntidadeFinanceiraService = TipoEntidadeFinanceiraService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetTipoEntidadeFinanceiraAsync(string keyword = "")
        {
            Response<IEnumerable<TipoEntidadeFinanceiraDTO>> result = await _TipoEntidadeFinanceiraService.GetTipoEntidadeFinanceiraAsync(keyword);
            return Ok(result);
        }

        // Lightweight List 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetTipoEntidadeFinanceiraLightAsync(string keyword = "")
        {
          Response<IEnumerable<TipoEntidadeFinanceiraLightDTO>> result = await _TipoEntidadeFinanceiraService.GetTipoEntidadeFinanceiraLightAsync(keyword);
          return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetTipoEntidadeFinanceiraPaginatedAsync(TipoEntidadeFinanceiraTableFilter filter)
        {
            PaginatedResponse<TipoEntidadeFinanceiraTableDTO> result = await _TipoEntidadeFinanceiraService.GetTipoEntidadeFinanceiraPaginatedAsync(filter);
            return Ok(result);
        }

        // All TipoEntidadeFinanceiras (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllTipoEntidadeFinanceiraAsync([FromBody] TipoEntidadeFinanceiraAllFilter filter)
        {
          try
          {
            Response<IEnumerable<TipoEntidadeFinanceiraTableDTO>> result = await _TipoEntidadeFinanceiraService.GetAllTipoEntidadeFinanceiraAsync(filter);
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
        public async Task<IActionResult> GetTipoEntidadeFinanceiraAsync(Guid id)
        {
            Response<TipoEntidadeFinanceiraDTO> result = await _TipoEntidadeFinanceiraService.GetTipoEntidadeFinanceiraAsync(id);
            return Ok(result);
        }

        // Single by Sigla
        [Authorize(Roles = "client")]
        [HttpGet("sigla/{sigla}")]
        public async Task<IActionResult> GetTipoEntidadeFinanceiraBySiglaAsync(string sigla)
        {
          Response<TipoEntidadeFinanceiraDTO> result = await _TipoEntidadeFinanceiraService.GetTipoEntidadeFinanceiraBySiglaAsync(sigla);
          return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateTipoEntidadeFinanceiraAsync(CreateTipoEntidadeFinanceiraRequest request)
        {
            try
            {
                Response<Guid> result = await _TipoEntidadeFinanceiraService.CreateTipoEntidadeFinanceiraAsync(request);
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
        public async Task<IActionResult> UpdateTipoEntidadeFinanceiraAsync(UpdateTipoEntidadeFinanceiraRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _TipoEntidadeFinanceiraService.UpdateTipoEntidadeFinanceiraAsync(request, id);
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
        public async Task<IActionResult> DeleteTipoEntidadeFinanceiraAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _TipoEntidadeFinanceiraService.DeleteTipoEntidadeFinanceiraAsync(id);
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
        public async Task<IActionResult> DeleteMultipleTipoEntidadeFinanceiraAsync([FromBody] DeleteMultipleTipoEntidadeFinanceiraRequest request)
        {
          try
          {
            Response<IEnumerable<Guid>> result = await _TipoEntidadeFinanceiraService.DeleteMultipleTipoEntidadeFinanceiraAsync(request.Ids);
            return Ok(result);
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        } 
    }
}
