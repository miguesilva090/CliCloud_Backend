using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico;

[Route("client/processo-clinico/[controller]")]
[ApiController]
public class SeparadorPersonalizadoController(
    ISeparadorPersonalizadoService service,
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly ISeparadorPersonalizadoService _service = service;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
        await _currentClinica.SetClinicaAsync();
        return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string keyword = "")
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<IEnumerable<SeparadorPersonalizadoDTO>> result =
            await _service.GetSeparadorPersonalizadoAsync(clinicaId.Value, keyword);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(
        [FromBody] SeparadorPersonalizadoTableFilter filter
    )
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        PaginatedResponse<SeparadorPersonalizadoDTO> result =
            await _service.GetSeparadorPersonalizadoPaginatedAsync(clinicaId.Value, filter);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<SeparadorPersonalizadoDTO> result =
            await _service.GetSeparadorPersonalizadoAsync(clinicaId.Value, id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSeparadorPersonalizadoRequest request)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<Guid> result = await _service.CreateSeparadorPersonalizadoAsync(clinicaId.Value, request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateSeparadorPersonalizadoRequest request,
        Guid id
    )
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<Guid> result = await _service.UpdateSeparadorPersonalizadoAsync(clinicaId.Value, request, id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<Guid> result = await _service.DeleteSeparadorPersonalizadoAsync(clinicaId.Value, id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] IEnumerable<Guid> ids)
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<IEnumerable<Guid>> result =
            await _service.DeleteMultipleSeparadorPersonalizadoAsync(clinicaId.Value, ids);
        return Ok(result);
    }
}
