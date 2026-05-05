using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Atestados.AtestadoService;
using CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using CliCloud.Application.Services.Atestados.AtestadoService.Filters;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common;

namespace CliCloud.WebApi.Controllers.Atestados
{
  [Route("client/atestados/[controller]")]
  [ApiController]
  public class AtestadoController(
    IAtestadoService atestadoService,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private readonly IAtestadoService _atestadoService = atestadoService;
    private readonly ICurrentClinicaService _currentClinica = currentClinica;

    private async Task<Guid?> ObterClinicaIdAsync()
    {
      await _currentClinica.SetClinicaAsync();
      if (string.IsNullOrWhiteSpace(_currentClinica.ClinicaId)) return null;
      return Guid.TryParse(_currentClinica.ClinicaId, out var id) ? id : null;
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetAtestadoPaginatedAsync(AtestadoTableFilter filter)
    {
      PaginatedResponse<AtestadoTableDTO> result = await _atestadoService.GetAtestadoPaginatedAsync(filter);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAllAtestadoAsync([FromBody] AtestadoAllFilter filter)
    {
      try
      {
        Response<IEnumerable<AtestadoTableDTO>> result = await _atestadoService.GetAllAtestadoAsync(filter);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAtestadoAsync(Guid id)
    {
      Response<AtestadoDTO> result = await _atestadoService.GetAtestadoAsync(id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id}/erro-comunicacao")]
    public async Task<IActionResult> ObterErroComunicacaoAsync(Guid id)
    {
      var clinicaId = await ObterClinicaIdAsync();
      if (clinicaId is null) return BadRequest("Clínica atual inválida.");
      var result = await _atestadoService.ObterErroComunicacaoAsync(id, clinicaId.Value);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAtestadoAsync(CreateAtestadoRequest request)
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
          return BadRequest("Sem clinica associada ao utilizador. Selecione uma clínica");
        
        request.ClinicaId = clinicaId.Value;

        Response<Guid> result = await _atestadoService.CreateAtestadoAsync(request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpPost("{id}/reenviar-offline")]
    public async Task<IActionResult> ReenviarAtestadoOfflineAsync(Guid id)
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida.");

        Response<Guid> result = await _atestadoService.ReenviarAtestadoOfflineAsync(id, clinicaId.Value);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpPost("reenviar-pendentes-offline")]
    public async Task<IActionResult> ReenviarPendentesOfflineAsync()
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null) return BadRequest("Clínica atual inválida.");

        Response<int> result = await _atestadoService.ReenviarPendentesOfflineAsync(clinicaId.Value);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAtestadoAsync(Guid id)
    {
      try
      {
        Response<Guid> response = await _atestadoService.DeleteAtestadoAsync(id);
        return Ok(response);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
