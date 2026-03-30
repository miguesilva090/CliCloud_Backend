using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.HistoriaClinica;

[Authorize]
[ApiController]
[Route("client/[controller]")]
public class HistoriaClinicaController : ControllerBase
{
  private readonly IHistoriaClinicaService _service;

  public HistoriaClinicaController(IHistoriaClinicaService service)
  {
    _service = service;
  }

  // Tabela paginada (TanStack Table)
  [HttpPost("table")]
  public async Task<ActionResult<PaginatedResponse<HistoriaClinicaTableDTO>>> GetTable(
    [FromBody] HistoriaClinicaTableFilter filter
  )
  {
    PaginatedResponse<HistoriaClinicaTableDTO> result =
      await _service.GetHistoriaClinicaPaginatedAsync(filter);

    return Ok(result);
  }

  // Detalhe por Id
  [HttpGet("{id:guid}")]
  public async Task<ActionResult<HistoriaClinicaDTO>> GetById(Guid id)
  {
    Response<HistoriaClinicaDTO> result = await _service.GetHistoriaClinicaAsync(id);
    if (result.Status != ResponseStatus.Success || result.Data is null)
    {
      return NotFound();
    }

    return Ok(result.Data);
  }

  // Criar
  [HttpPost]
  public async Task<ActionResult<Guid>> Create([FromBody] CreateHistoriaClinicaRequest request)
  {
    Response<Guid> result = await _service.CreateHistoriaClinicaAsync(request);
    if (result.Status != ResponseStatus.Success || result.Data == Guid.Empty)
    {
      return BadRequest(result);
    }

    return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
  }

  // Atualizar
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHistoriaClinicaRequest request)
  {
    Response<Guid> result = await _service.UpdateHistoriaClinicaAsync(request, id);
    if (result.Status != ResponseStatus.Success)
    {
      return BadRequest(result);
    }

    return NoContent();
  }

  // Apagar único
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    Response<Guid> result = await _service.DeleteHistoriaClinicaAsync(id);
    if (result.Status != ResponseStatus.Success)
    {
      return BadRequest(result);
    }

    return NoContent();
  }
}

﻿