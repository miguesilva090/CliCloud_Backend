using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs;
using CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.Tratamentos
{
    [Route("client/tratamentos/[controller]")]
    [ApiController]
    public class MotivosDesmarcacaoController(IMotivosDesmarcacaoService MotivosDesmarcacaoService) : ControllerBase
    {
        private readonly IMotivosDesmarcacaoService _MotivosDesmarcacaoService = MotivosDesmarcacaoService;

        // full list
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMotivosDesmarcacaoAsync(string keyword = "")
        {
            Response<IEnumerable<MotivosDesmarcacaoDTO>> result = await _MotivosDesmarcacaoService.GetMotivosDesmarcacaoAsync(keyword);
            return Ok(result);
        }

        // lightweight list
        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMotivosDesmarcacaoLightAsync(string keyword = "")
        {
            Response<IEnumerable<MotivosDesmarcacaoLightDTO>> result = await _MotivosDesmarcacaoService.GetMotivosDesmarcacaoLightAsync(keyword);
            return Ok(result);
        }

        // paginated & filtered list
        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMotivosDesmarcacaoPaginatedAsync(MotivosDesmarcacaoTableFilter filter)
        {
            PaginatedResponse<MotivosDesmarcacaoTableDTO> result = await _MotivosDesmarcacaoService.GetMotivosDesmarcacaoPaginatedAsync(filter);
            return Ok(result);
        }

        // all non-paginated list
        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMotivosDesmarcacaoAsync(MotivosDesmarcacaoAllFilter filter)
        {
            try
            {
                Response<IEnumerable<MotivosDesmarcacaoTableDTO>> result = await _MotivosDesmarcacaoService.GetAllMotivosDesmarcacaoAsync(filter);
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
        public async Task<IActionResult> GetMotivosDesmarcacaoAsync(Guid id)
        {
            Response<MotivosDesmarcacaoDTO> result = await _MotivosDesmarcacaoService.GetMotivosDesmarcacaoAsync(id);
            return Ok(result);
        }

        // single by Descricao
        [Authorize(Roles = "client")]
        [HttpGet("descricao/{descricao}")]
        public async Task<IActionResult> GetMotivosDesmarcacaoByDescricaoAsync(string descricao)
        {
            Response<MotivosDesmarcacaoDTO> result = await _MotivosDesmarcacaoService.GetMotivosDesmarcacaoByDescricaoAsync(descricao);
            return Ok(result);
        }

        // create
        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivosDesmarcacaoAsync(CreateMotivosDesmarcacaoRequest request)
        {
            try
            {
                Response<Guid> result = await _MotivosDesmarcacaoService.CreateMotivosDesmarcacaoAsync(request);
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
        public async Task<IActionResult> UpdateMotivosDesmarcacaoAsync(UpdateMotivosDesmarcacaoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _MotivosDesmarcacaoService.UpdateMotivosDesmarcacaoAsync(request, id);
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
        public async Task<IActionResult> DeleteMotivosDesmarcacaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _MotivosDesmarcacaoService.DeleteMotivosDesmarcacaoAsync(id);
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
        public async Task<IActionResult> DeleteMultipleMotivosDesmarcacaoAsync([FromBody] DeleteMultipleMotivosDesmarcacaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _MotivosDesmarcacaoService.DeleteMultipleMotivosDesmarcacaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
