using CliCloud.Application.Common;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService;
using CliCloud.Application.Services.Faturacao.WebserviceAdseService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/adse")]
[ApiController]
[Authorize(Roles = "client")]
public class AdseController(
    IWebserviceAdseService service,
    ICurrentClinicaService currentClinicaService
) : ControllerBase
{
    private readonly IWebserviceAdseService _service = service;
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
    public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarWebserviceAdseRequest request)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.GuardarConfiguracaoAsync(clinicaId.Value, request));
    }

    [HttpGet("lookups/organismos")]
    public async Task<IActionResult> ListarOrganismosAsync()
        => Ok(await _service.ListarOrganismosAsync());
}