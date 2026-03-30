using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs;
using CliCloud.Application.Services.Servicos.SubsistemaServicoService.Filters;

namespace CliCloud.WebApi.Controllers.Servicos
{
  [Route("client/servicos/[controller]")]
  [ApiController]
  public class SubsistemaServicoController(ISubsistemaServicoService service) : ControllerBase
  {
    private readonly ISubsistemaServicoService _service = service;

    // Lista completa (opcionalmente filtrada por serviço)
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetSubsistemaServicoAsync(Guid? servicoId = null)
    {
      Response<IEnumerable<SubsistemaServicoDTO>> result = await _service.GetSubsistemaServicoAsync(servicoId);
      return Ok(result);
    }

    // Lista paginada (usada pela tabela)
    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetSubsistemaServicoPaginatedAsync(SubsistemaServicoTableFilter filter)
    {
      PaginatedResponse<SubsistemaServicoDTO> result =
        await _service.GetSubsistemaServicoPaginatedAsync(filter);
      return Ok(result);
    }

    // Detalhe por Id
    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSubsistemaServicoAsync(Guid id)
    {
      Response<SubsistemaServicoDTO> result = await _service.GetSubsistemaServicoAsync(id);
      return Ok(result);
    }

    // Criar
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateSubsistemaServicoAsync(CreateSubsistemaServicoRequest request)
    {
      try
      {
        Response<Guid> result = await _service.CreateSubsistemaServicoAsync(request);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // Atualizar
    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSubsistemaServicoAsync(Guid id, UpdateSubsistemaServicoRequest request)
    {
      try
      {
        Response<Guid> result = await _service.UpdateSubsistemaServicoAsync(request, id);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // Eliminar
    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSubsistemaServicoAsync(Guid id)
    {
      try
      {
        Response<Guid> result = await _service.DeleteSubsistemaServicoAsync(id);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}

