using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MotivoConsultaService;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas
{
  [Route("client/consultas/[controller]")]
  [ApiController]
  [Authorize(Roles = "client")]
  public class MotivoConsultaController : ControllerBase
  {
    private readonly IMotivoConsultaService _motivoConsultaService;

    public MotivoConsultaController(IMotivoConsultaService motivoConsultaService)
    {
      _motivoConsultaService = motivoConsultaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var result = await _motivoConsultaService.GetAllAsync();
      return Ok(result);
    }

    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync([FromBody] MotivoConsultaTableFilter filter)
    {
      PaginatedResponse<MotivoConsultaTableDTO> result = await _motivoConsultaService.GetPaginatedAsync(filter);
      return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<MotivoConsultaDTO> result = await _motivoConsultaService.GetByIdAsync(id);
      return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMotivoConsultaRequest request)
    {
      Response<Guid> result = await _motivoConsultaService.CreateAsync(request);
      return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateMotivoConsultaRequest request, Guid id)
    {
      Response<Guid> result = await _motivoConsultaService.UpdateAsync(request, id);
      return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _motivoConsultaService.DeleteAsync(id);
      return Ok(result);
    }

    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] DeleteMultipleMotivoConsultaRequest request)
    {
      Response<IEnumerable<Guid>> result = await _motivoConsultaService.DeleteMultipleAsync(request.Ids);
      return Ok(result);
    }
  }
}
