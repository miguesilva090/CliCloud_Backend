using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService;
using CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs;

namespace CliCloud.WebApi.Controllers.ProcessoClinico 
{
    [Route("client/processo-clinico/[controller]")]
    [ApiController]
    public class RelatorioExamesController(IRelatorioExamesService relatorioExamesService, ICurrentTenantUserService currentTenantUserService) : ControllerBase
    {
        private readonly IRelatorioExamesService _relatorioExamesService = relatorioExamesService;
        private readonly ICurrentTenantUserService _currentTenantUserService = currentTenantUserService;

        [Authorize(Roles = "client")]
        [HttpGet("{utenteId}")]
        public async Task<IActionResult> GetRelatorioExamesAsync(Guid utenteId)
        {
            if (!Guid.TryParse(_currentTenantUserService.UserId, out var userId))
                return BadRequest("Utilizador inválido");

            Response<RelatorioExamesDTO?> result = 
                await _relatorioExamesService.GetByUtenteAndMedicoAsync( utenteId, userId );
            return Ok(result);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> UpsertRelatorioExamesAsync([FromBody] UpdateRelatorioExamesRequest request)
        {
            if (!Guid.TryParse(_currentTenantUserService.UserId, out var userId))
                return BadRequest("Utilizador inválido");

            Response<Guid> result = await _relatorioExamesService.UpdateRelatorioExamesAsync(request, userId);
            return Ok(result);
        }
    }
}