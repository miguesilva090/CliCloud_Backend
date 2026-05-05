using CliCloud.Application.Common;
using CliCloud.Application.Services.Utentes.UtenteRnuService;
using CliCloud.Application.Services.Utentes.UtenteRnuService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Utentes;

[Route("client/utentes/[controller]")]
[ApiController]
[Authorize(Roles = "client")]
public class UtenteRnuController(
    IUtenteRnuService utenteRnuService,
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly IUtenteRnuService _utenteRnuService = utenteRnuService;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    [HttpPost("consultar")]
    public async Task<IActionResult> ConsultarAsync([FromBody] ConsultarUtenteRnuRequest request)
    {
        await _currentClinica.SetClinicaAsync();

        if(!Guid.TryParse(_currentClinica.ClinicaId, out var clinicaId))
            return BadRequest("Clínica atual inválida");
        
        var response = await _utenteRnuService.ConsultarUtenteAsync(clinicaId, request);
        return Ok(response);
    }
}