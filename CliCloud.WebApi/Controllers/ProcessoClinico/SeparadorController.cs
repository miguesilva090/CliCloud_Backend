using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.SeparadorService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico;

[Route("client/processo-clinico/[controller]")]
[ApiController]
public class SeparadorController(
    ISeparadorService service,
    ICurrentClinicaService currentClinica
) : ControllerBase
{
    private readonly ISeparadorService _service = service;
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
        Response<IEnumerable<SeparadorDTO>> result = await _service.GetSeparadorAsync(keyword);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync([FromBody] SeparadorTableFilter filter)
    {
        PaginatedResponse<SeparadorDTO> result = await _service.GetSeparadorPaginatedAsync(filter);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("ficha-clinica-visiveis")]
    public async Task<IActionResult> GetFichaClinicaVisiveisAsync(
        [FromQuery] Guid? medicoId = null,
        [FromQuery] Guid? especialidadeId = null
    )
    {
        Guid? clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
        {
            return BadRequest("Clínica atual inválida");
        }

        Response<IEnumerable<SeparadorFichaClinicaDTO>> result =
            await _service.GetSeparadoresFichaClinicaVisiveisAsync(
                clinicaId.Value,
                medicoId,
                especialidadeId
            );
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        Response<SeparadorDTO> result = await _service.GetSeparadorAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSeparadorRequest request)
    {
        Response<Guid> result = await _service.CreateSeparadorAsync(request);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateSeparadorRequest request, Guid id)
    {
        Response<Guid> result = await _service.UpdateSeparadorAsync(request, id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        Response<Guid> result = await _service.DeleteSeparadorAsync(id);
        return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] IEnumerable<Guid> ids)
    {
        Response<IEnumerable<Guid>> result = await _service.DeleteMultipleSeparadorAsync(ids);
        return Ok(result);
    }
}
