using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs;
using CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.Filters;

namespace CliCloud.WebApi.Controllers.TaxasIva
{
    [Route("client/taxas-iva/[controller]")]
    [ApiController]
    public class MotivoRetencaoController(IMotivoRetencaoService motivoRetencaoService) : ControllerBase
    {
        private readonly IMotivoRetencaoService _service = motivoRetencaoService;

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(string keyword = "")
            => Ok(await _service.GetMotivoRetencaoAsync(keyword));

        [Authorize(Roles = "client")]
        [HttpGet("light")]
        public async Task<IActionResult> GetLightAsync(string keyword = "", string? tipoImposto = null)
            => Ok(await _service.GetMotivoRetencaoLightAsync(keyword, tipoImposto));

        [Authorize(Roles = "client")]
        [HttpPost("paginated")]
        public async Task<IActionResult> GetPaginatedAsync(MotivoRetencaoTableFilter filter)
            => Ok(await _service.GetMotivoRetencaoPaginatedAsync(filter));

        [Authorize(Roles = "client")]
        [HttpPost("all")]
        public async Task<IActionResult> GetAllAsync([FromBody] MotivoRetencaoAllFilter? filter = null)
            => Ok(await _service.GetAllMotivoRetencaoAsync(filter ?? new MotivoRetencaoAllFilter()));

        [Authorize(Roles = "client")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
            => Ok(await _service.GetMotivoRetencaoAsync(id));

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMotivoRetencaoRequest request)
            => Ok(await _service.CreateMotivoRetencaoAsync(request));

        [Authorize(Roles = "client")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(UpdateMotivoRetencaoRequest request, Guid id)
            => Ok(await _service.UpdateMotivoRetencaoAsync(request, id));

        [Authorize(Roles = "client")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
            => Ok(await _service.DeleteMotivoRetencaoAsync(id));

        [Authorize(Roles = "client")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleMotivoRetencaoRequest request)
            => Ok(await _service.DeleteMultipleMotivoRetencaoAsync(request.Ids));
    }
}
