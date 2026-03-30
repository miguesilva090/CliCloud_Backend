using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService;
using CliCloud.Application.Services.UnidadesLocaisSaude.UnidadesLocaisSaudeService.DTOs;

namespace CliCloud.WebApi.Controllers.UnidadesLocaisSaude
{
  [Route("client/unidades-locais-saude/[controller]")]
  [ApiController]
  public class UnidadesLocaisSaudeController(IUnidadesLocaisSaudeService unidadesLocaisSaudeService) : ControllerBase
  {
    private readonly IUnidadesLocaisSaudeService _unidadesLocaisSaudeService = unidadesLocaisSaudeService;

    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetUnidadesLocaisSaudeLightAsync(string keyword = "")
    {
      Response<IEnumerable<UnidadesLocaisSaudeLightDTO>> result =
        await _unidadesLocaisSaudeService.GetUnidadesLocaisSaudeLightAsync(keyword);

      return Ok(result);
    }
  }
}

