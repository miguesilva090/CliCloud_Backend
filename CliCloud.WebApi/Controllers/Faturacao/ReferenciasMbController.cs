using CliCloud.Application.Common;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService;
using CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/referencias-mb")]
[ApiController]
[Authorize(Roles = "client")]
public class ReferenciasMbController(
    IReferenciasMbService service,
    ICurrentClinicaService currentClinicaService
) : ControllerBase
{
    private readonly IReferenciasMbService _service = service;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        return Guid.TryParse(_currentClinicaService.ClinicaId, out Guid id) ? id : null;
    }

    [HttpGet("configuracao")]
    public async Task<IActionResult> ObterConfiguracaoAtualAsync()
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.ObterConfiguracaoAtualAsync(clinicaId.Value));
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarConfigReferenciaMbRequest request)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.GuardarConfiguracaoAsync(clinicaId.Value, request));
    }

    [HttpGet("configuracao/callback-ifthen")]
    public async Task<IActionResult> ObterCallbackIfThenAsync()
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.ConstruirCallbackIfThenAsync(clinicaId.Value));
    }

    [HttpGet("historico")]
    public async Task<IActionResult> ListarHistoricoAsync()
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.ListarHistoricoAsync(clinicaId.Value));
    }

    [HttpPost("{id:guid}/liquidar")]
    public async Task<IActionResult> LiquidarAsync(Guid id)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.MarcarLiquidadaAsync(clinicaId.Value, id));
    }

    [HttpPost("{id:guid}/anular")]
    public async Task<IActionResult> AnularAsync(Guid id, [FromBody] AnularReferenciaMbRequest request)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.AnularAsync(clinicaId.Value, id, request));
    }

    [AllowAnonymous]
    [HttpPost("callback/ifthen")]
    public async Task<IActionResult> CallbackIfThenAsync([FromQuery] IfThenCallbackRequest request)
    {
        return Ok(await _service.ReceberCallbackIfThenAsync(request));
    }
}
