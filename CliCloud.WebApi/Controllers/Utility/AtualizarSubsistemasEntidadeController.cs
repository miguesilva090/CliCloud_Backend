using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService;
using CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService.DTOs;

namespace CliCloud.WebApi.Controllers.Utility;

[Route("client/utility/atualizar-subsistemas-entidade")]
[ApiController]
public class AtualizarSubsistemasEntidadeController(IAtualizarSubsistemasEntidadeService service) : ControllerBase
{
    private readonly IAtualizarSubsistemasEntidadeService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarSubsistemasEntidadeRequest request)
    {
        var result = await _service.AtualizarAsync(request);
        return Ok(result);
    }
}
