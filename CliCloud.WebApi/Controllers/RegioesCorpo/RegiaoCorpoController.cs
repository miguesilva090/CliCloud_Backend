using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs;
using CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.RegioesCorpo
{
    [Route("client/regioes-corpo/[controller]")]
    [ApiController]
    public class RegiaoCorpoController(IRegiaoCorpoService RegiaoCorpoService) : ControllerBase
    {
        private readonly IRegiaoCorpoService _RegiaoCorpoService = RegiaoCorpoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetRegiaoCorpoAsync(string keyword = "")
        {
            Response<IEnumerable<RegiaoCorpoDTO>> result = await _RegiaoCorpoService.GetRegiaoCorpoAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetRegiaoCorpoLightAsync(string keyword = "")
        {
            Response<IEnumerable<RegiaoCorpoLightDTO>> result = await _RegiaoCorpoService.GetRegiaoCorpoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetRegiaoCorpoPaginatedAsync(RegiaoCorpoTableFilter filter)
        {
            PaginatedResponse<RegiaoCorpoTableDTO> result = await _RegiaoCorpoService.GetRegiaoCorpoPaginatedAsync(filter);
            return Ok(result);
        }

        // all RegiaoCorpo (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllRegiaoCorpoAsync([FromBody] RegiaoCorpoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<RegiaoCorpoTableDTO>> result = await _RegiaoCorpoService.GetAllRegiaoCorpoAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // single by Id
        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegiaoCorpoAsync(Guid id)
        {
            Response<RegiaoCorpoDTO> result = await _RegiaoCorpoService.GetRegiaoCorpoAsync(id);
            return Ok(result);
        }

        //Single by Descricao
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetRegiaoCorpoByDescricaoAsync(string descricao)
        {
            Response<RegiaoCorpoDTO> result = await _RegiaoCorpoService.GetRegiaoCorpoByDescricaoAsync(descricao);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateRegiaoCorpoAsync(CreateRegiaoCorpoRequest request)
        {
            try
            {
                Response<Guid> result = await _RegiaoCorpoService.CreateRegiaoCorpoAsync(request);
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
        public async Task<IActionResult> UpdateRegiaoCorpoAsync(UpdateRegiaoCorpoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _RegiaoCorpoService.UpdateRegiaoCorpoAsync(request, id);
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
        public async Task<IActionResult> DeleteRegiaoCorpoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _RegiaoCorpoService.DeleteRegiaoCorpoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleRegiaoCorpoAsync([FromBody] DeleteMultipleRegiaoCorpoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> response = await _RegiaoCorpoService.DeleteMultipleRegiaoCorpoAsync(request.Ids);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
