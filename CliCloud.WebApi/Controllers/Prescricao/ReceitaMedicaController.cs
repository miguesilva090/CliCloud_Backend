using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs;
using CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Filters;

namespace CliCloud.WebApi.Controllers.Prescricao
{
  [Route("client/prescricao/[controller]")]
  [ApiController]
  public class ReceitaMedicaController(
    IReceitaMedicaService receitaMedicaService,
    ICurrentClinicaService currentClinica
  ) : ControllerBase
  {
    private async Task<Guid?> ObterClinicaIdAsync()
    {
      await currentClinica.SetClinicaAsync();
      if (string.IsNullOrWhiteSpace(currentClinica.ClinicaId)) return null;
      return Guid.TryParse(currentClinica.ClinicaId, out var id) ? id : null;
    }

    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(ReceitaMedicaTableFilter filter)
    {
      PaginatedResponse<ReceitaMedicaTableDTO> result = await receitaMedicaService.GetPaginatedAsync(filter);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<ReceitaMedicaDTO> result = await receitaMedicaService.GetByIdAsync(id);
      return Ok(result);
    }

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateReceitaMedicaRequest request)
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
          return BadRequest("Sem clinica associada ao utilizador. Selecione uma clínica");

        request.ClinicaId = clinicaId.Value;
        Response<Guid> result = await receitaMedicaService.CreateAsync(request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateReceitaMedicaRequest request)
    {
      try
      {
        Response<Guid> result = await receitaMedicaService.UpdateAsync(id, request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpPost("{id:guid}/enviar")]
    public async Task<IActionResult> EnviarAsync(Guid id, [FromBody] EnviarReceitaMedicaRequest? request)
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
          return BadRequest("Sem clinica associada ao utilizador. Selecione uma clínica");

        request ??= new EnviarReceitaMedicaRequest();
        Response<Guid> result = await receitaMedicaService.EnviarAsync(id, clinicaId.Value, request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Roles = "client")]
    [HttpPost("{id:guid}/anular")]
    public async Task<IActionResult> AnularAsync(Guid id, AnularReceitaMedicaRequest request)
    {
      try
      {
        var clinicaId = await ObterClinicaIdAsync();
        if (clinicaId is null)
          return BadRequest("Sem clinica associada ao utilizador. Selecione uma clínica");

        Response<Guid> result = await receitaMedicaService.AnularAsync(id, request, clinicaId.Value);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
