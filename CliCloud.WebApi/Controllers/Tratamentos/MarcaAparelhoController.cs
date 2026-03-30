using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class MarcaAparelhoController(IMarcaAparelhoService MarcaAparelhoService) : ControllerBase
    {
        private readonly IMarcaAparelhoService _MarcaAparelhoService = MarcaAparelhoService;
        
        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMarcaAparelhoAsync(string keyword = "")
        {
            Response<IEnumerable<MarcaAparelhoDTO>> result = await _MarcaAparelhoService.GetMarcaAparelhoAsync(keyword);
            return Ok(result);
        }

        // lightweight list 
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMarcaAparelhoLightAsync(string keyword = "")
        {
            Response<IEnumerable<MarcaAparelhoLightDTO>> result = await _MarcaAparelhoService.GetMarcaAparelhoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMarcaAparelhoPaginatedAsync(MarcaAparelhoTableFilter filter)
        {
            PaginatedResponse<MarcaAparelhoTableDTO> result = await _MarcaAparelhoService.GetMarcaAparelhoPaginatedAsync(filter);
            return Ok(result);
        }

        // all list (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMarcaAparelhoAsync([FromBody] MarcaAparelhoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<MarcaAparelhoTableDTO>> result = await _MarcaAparelhoService.GetAllMarcaAparelhoAsync(filter);
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
        public async Task<IActionResult> GetMarcaAparelhoAsync(Guid id)
        {
            Response<MarcaAparelhoDTO> result = await _MarcaAparelhoService.GetMarcaAparelhoAsync(id);
            return Ok(result);
        }

        //single by Designacao (exact match)
        [Authorize(Roles = "client")]
        [HttpGet("designacao/{designacao}")]
        public async Task<IActionResult> GetMarcaAparelhoByDesignacaoAsync(string designacao)
        {
            try
            {
            Response<MarcaAparelhoDTO> result = await _MarcaAparelhoService.GetMarcaAparelhoByDesignacaoAsync(designacao);
            return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMarcaAparelhoAsync(CreateMarcaAparelhoRequest request)
        {
            try
            {
                Response<Guid> result = await _MarcaAparelhoService.CreateMarcaAparelhoAsync(request);
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
        public async Task<IActionResult> UpdateMarcaAparelhoAsync(UpdateMarcaAparelhoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MarcaAparelhoService.UpdateMarcaAparelhoAsync(request, id);
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
        public async Task<IActionResult> DeleteMarcaAparelhoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MarcaAparelhoService.DeleteMarcaAparelhoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleMarcaAparelhoAsync([FromBody] DeleteMultipleMarcaAparelhoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _MarcaAparelhoService.DeleteMultipleMarcaAparelhoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
