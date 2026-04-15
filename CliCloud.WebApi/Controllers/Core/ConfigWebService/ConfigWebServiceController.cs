using CliCloud.Application.Common;
using CliCloud.Application.Services.Core.ConfigWebServiceService;
using CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService;
using CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Core.ConfigWebService;

[Route("client/core/[controller]")]
[ApiController]
[Authorize(Roles = "client")]
public class ConfigWebServiceController(
    IConfigWebServiceService service,
    ISpmsPrescricaoSoapService spms, 
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly IConfigWebServiceService _service = service;
    private readonly ISpmsPrescricaoSoapService _spms = spms;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinica.SetClinicaAsync();
        return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [HttpGet("configuracao")]
    public async Task<IActionResult> ObterConfiguracaoAtualAsync()
    {
        var clinicaId = await ObterClinicaIdAsync();
        if(clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.ObterConfiguracaoAtualAsync(clinicaId.Value));
    }

    [HttpPut("configuracao")]
    public async Task<IActionResult> GuardarConfiguracaoAsync([FromBody] AtualizarConfigWebServiceRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if(clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _service.GuardarConfiguracaoAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-token-cred")]
    public async Task<IActionResult> TestarTokenCredAsync([FromBody] ObterTokenCredRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if(clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ObterTokenCredAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-token-cc")]
    public async Task<IActionResult> TestarTokenCcAsync([FromBody] ObterTokenAssinadoRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ObterTokenCcAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-token-com")]
    public async Task<IActionResult> TestarTokenComAsync([FromBody] ObterTokenAssinadoRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ObterTokenComAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-consulta-utente")]
    public async Task<IActionResult> TestarConsultaUtenteAsync([FromBody] ConsultaUtenteRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ExecutarConsultaUtenteAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-registo-prescricao")]
    public async Task<IActionResult> TestarRegistoPrescricaoAsync([FromBody] RegistoPrescricaoRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ExecutarRegistoPrescricaoAsync(clinicaId.Value, request));
    }

    [HttpPost("testar-registo-prescricao-rsp")]
    public async Task<IActionResult> TestarRegistoPrescricaoRspAsync([FromBody] RegistoPrescricaoRspRequest request)
    {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida");
        return Ok(await _spms.ExecutarRegistoPrescricaoRspAsync(clinicaId.Value, request));
    }
}