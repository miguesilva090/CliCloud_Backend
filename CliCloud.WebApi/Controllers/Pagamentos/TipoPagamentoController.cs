using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Pagamentos.TipoPagamentoService;

namespace CliCloud.WebApi.Controllers.Pagamentos;

[Route("client/pagamentos/[controller]")]
[ApiController]
public class TipoPagamentoController(ITipoPagamentoService service) : ControllerBase
{
    [Authorize(Roles = "client")]
    [HttpGet("light")]
    public async Task<IActionResult> GetLightAsync(string keyword = "")
        => Ok(await service.GetLightAsync(keyword));
}
