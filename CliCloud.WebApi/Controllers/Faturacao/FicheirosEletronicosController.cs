using CliCloud.Application.Common;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;
using CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/ficheiros-eletronicos")]
[ApiController]
[Authorize(Roles = "client")]
public class FicheirosEletronicosController(
    IFicheirosEletronicosService service,
    ICurrentClinicaService currentClinicaService
) : ControllerBase
{
    private readonly IFicheirosEletronicosService _service = service;
    private readonly ICurrentClinicaService _currentClinicaService = currentClinicaService;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinicaService.SetClinicaAsync();
        return Guid.TryParse(_currentClinicaService.ClinicaId, out Guid id) ? id : null;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAsync([FromQuery] string sigla, CancellationToken ct)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida.");
        return Ok(await _service.ListarAsync(clinicaId.Value, sigla, ct));
    }

    [HttpPost("gerar")]
    public async Task<IActionResult> GerarAsync([FromBody] GerarFicheiroEletronicoRequest request, CancellationToken ct)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida.");
        return Ok(await _service.GerarAsync(clinicaId.Value, request, ct));
    }

    [HttpPost("juntar")]
    public async Task<IActionResult> JuntarAsync([FromBody] JuntarFicheirosEletronicosRequest request, CancellationToken ct)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida.");
        return Ok(await _service.JuntarAsync(clinicaId.Value, request, ct));
    }
}