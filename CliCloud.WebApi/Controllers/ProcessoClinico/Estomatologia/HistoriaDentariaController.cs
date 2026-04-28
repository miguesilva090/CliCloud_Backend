using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService;
using CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.ProcessoClinico.Estomatologia
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class HistoriaDentariaController(
        IHistoriaDentariaService historiaDentariaService,
        ICurrentTenantUserService currentTenantUserService
    ) : ControllerBase
    {
        private readonly IHistoriaDentariaService _historiaDentariaService = historiaDentariaService;
        private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;

        [Authorize(Roles = "client")]
        [HttpGet("{utenteId}")]
        public async Task<IActionResult> GetByUtenteAsync(Guid utenteId)
        {
            Response<IReadOnlyList<HistoriaDentariaDTO>> result =
                await _historiaDentariaService.GetByUtenteAsync(utenteId);
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateHistoriaDentariaRequest request)
        {
            if (!Guid.TryParse(_currentTenantUserService.UserId, out var userId))
                return BadRequest("Utilizador inválido");

            Response<Guid> result = await _historiaDentariaService.CreateAsync(request, userId);
            return Ok(result);
        }
    }
}
