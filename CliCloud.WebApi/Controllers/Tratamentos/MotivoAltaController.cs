using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs;
using CliCloud.Application.Services.Tratamentos.MotivoAltaService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class MotivoAltaController(IMotivoAltaService MotivoAltaService) : ControllerBase
    {
        private readonly IMotivoAltaService _MotivoAltaService = MotivoAltaService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMotivoAltaAsync(string keyword = "")
        {
            Response<IEnumerable<MotivoAltaDTO>> result = await _MotivoAltaService.GetMotivoAltaAsync(keyword);
            return Ok(result);
        }

         // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMotivoAltaLightAsync(string keyword = "")
        {
            Response<IEnumerable<MotivoAltaLightDTO>> result = await _MotivoAltaService.GetMotivoAltaLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMotivoAltaPaginatedAsync(MotivoAltaTableFilter filter)
        {
            PaginatedResponse<MotivoAltaTableDTO> result = await _MotivoAltaService.GetMotivoAltaPaginatedAsync(filter);
            return Ok(result);
        }

        // all (non-paginated)
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMotivoAltaAsync([FromBody] MotivoAltaAllFilter filter)
        {
            try
            {
                Response<IEnumerable<MotivoAltaTableDTO>> result = await _MotivoAltaService.GetAllMotivoAltaAsync(filter);
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
        public async Task<IActionResult> GetMotivoAltaAsync(Guid id)
        {
            Response<MotivoAltaDTO> result = await _MotivoAltaService.GetMotivoAltaAsync(id);
            return Ok(result);
        }

        //single by Descricao
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetMotivoAltaByDescricaoAsync(string descricao)
        {
            Response<MotivoAltaDTO> result = await _MotivoAltaService.GetMotivoAltaByDescricaoAsync(descricao);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivoAltaAsync(CreateMotivoAltaRequest request)
        {
            try
            {
                Response<Guid> result = await _MotivoAltaService.CreateMotivoAltaAsync(request);
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
        public async Task<IActionResult> UpdateMotivoAltaAsync(UpdateMotivoAltaRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MotivoAltaService.UpdateMotivoAltaAsync(request, id);
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
        public async Task<IActionResult> DeleteMotivoAltaAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MotivoAltaService.DeleteMotivoAltaAsync(id);
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
        public async Task<IActionResult> DeleteMultipleMotivoAltaAsync([FromBody] DeleteMultipleMotivoAltaRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _MotivoAltaService.DeleteMultipleMotivoAltaAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
