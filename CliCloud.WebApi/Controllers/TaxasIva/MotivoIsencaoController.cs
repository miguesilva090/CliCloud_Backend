using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Filters;
using CliCloud.Application.Common.Wrapper;

namespace CliCloud.WebApi.Controllers.TaxasIva
{
    [Route("client/taxas-iva/[controller]")]
    [ApiController]
    public class MotivoIsencaoController(IMotivoIsencaoService motivoIsencaoService) : ControllerBase
    {
        private readonly IMotivoIsencaoService _motivoIsencaoService = motivoIsencaoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetMotivoIsencaoAsync(string keyword = "")
        {
            Response<IEnumerable<MotivoIsencaoDTO>> result = await _motivoIsencaoService.GetMotivoIsencaoAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetMotivoIsencaoLightAsync(string keyword = "")
        {
            Response<IEnumerable<MotivoIsencaoLightDTO>> result = await _motivoIsencaoService.GetMotivoIsencaoLightAsync(keyword);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetMotivoIsencaoPaginatedAsync(MotivoIsencaoTableFilter filter)
        {
            PaginatedResponse<MotivoIsencaoTableDTO> result = await _motivoIsencaoService.GetMotivoIsencaoPaginatedAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllMotivoIsencaoAsync([FromBody] MotivoIsencaoAllFilter? filter = null)
        {
            try
            {
                Response<IEnumerable<MotivoIsencaoTableDTO>> result = await _motivoIsencaoService.GetAllMotivoIsencaoAsync(filter ?? new MotivoIsencaoAllFilter());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotivoIsencaoAsync(Guid id)
        {
            Response<MotivoIsencaoDTO> result = await _motivoIsencaoService.GetMotivoIsencaoAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateMotivoIsencaoAsync(CreateMotivoIsencaoRequest request)
        {
            try
            {
                Response<Guid> result = await _motivoIsencaoService.CreateMotivoIsencaoAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMotivoIsencaoAsync(UpdateMotivoIsencaoRequest request, Guid id)
        {
            try
            {
                Response<Guid> result = await _motivoIsencaoService.UpdateMotivoIsencaoAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotivoIsencaoAsync(Guid id)
        {
            try
            {
                Response<Guid> response = await _motivoIsencaoService.DeleteMotivoIsencaoAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleMotivoIsencaoAsync([FromBody] DeleteMultipleMotivoIsencaoRequest request)
        {
            try
            {
                Response<IEnumerable<Guid>> result = await _motivoIsencaoService.DeleteMultipleMotivoIsencaoAsync(request.Ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
