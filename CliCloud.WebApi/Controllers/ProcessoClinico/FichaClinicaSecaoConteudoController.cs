using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.DTOs;
using CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoConteudoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloudWebApi.Controllers.ProcessoClinico
{
  [Route("client/processo-clinico/[controller]")]
  [ApiController]
  public class FichaClinicaSecaoConteudoController(IFichaClinicaSecaoConteudoService service)
    : ControllerBase
  {
    private readonly IFichaClinicaSecaoConteudoService _service = service;

    // Lista simples (se precisares, para debug/admin)
    [Authorize(Roles = "client")]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string keyword = "")
    {
      Response<IEnumerable<FichaClinicaSecaoConteudoDTO>> result =
        await _service.GetFichaClinicaSecaoConteudoAsync(keyword);
      return Ok(result);
    }

    // Tabela paginada
    [Authorize(Roles = "client")]
    [HttpPost("paginated")]
    public async Task<IActionResult> GetPaginatedAsync(
      [FromBody] FichaClinicaSecaoConteudoTableFilter filter
    )
    {
      PaginatedResponse<FichaClinicaSecaoConteudoDTO> result =
        await _service.GetFichaClinicaSecaoConteudoPaginatedAsync(filter);
      return Ok(result);
    }

    // Detalhe por Id
    [Authorize(Roles = "client")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
      Response<FichaClinicaSecaoConteudoDTO> result =
        await _service.GetFichaClinicaSecaoConteudoAsync(id);
      return Ok(result);
    }

    // Criar
    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
      [FromBody] CreateFichaClinicaSecaoConteudoRequest request
    )
    {
      Response<Guid> result = await _service.CreateFichaClinicaSecaoConteudoAsync(request);
      return Ok(result);
    }

    // Atualizar
    [Authorize(Roles = "client")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
      [FromBody] UpdateFichaClinicaSecaoConteudoRequest request,
      Guid id
    )
    {
      Response<Guid> result = await _service.UpdateFichaClinicaSecaoConteudoAsync(request, id);
      return Ok(result);
    }

    // Apagar único
    [Authorize(Roles = "client")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
      Response<Guid> result = await _service.DeleteFichaClinicaSecaoConteudoAsync(id);
      return Ok(result);
    }

    // Apagar múltiplos (bulk)
    [Authorize(Roles = "client")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> DeleteMultipleAsync([FromBody] IEnumerable<Guid> ids)
    {
      Response<IEnumerable<Guid>> result =
        await _service.DeleteMultipleFichaClinicaSecaoConteudoAsync(ids);
      return Ok(result);
    }
  }
}

