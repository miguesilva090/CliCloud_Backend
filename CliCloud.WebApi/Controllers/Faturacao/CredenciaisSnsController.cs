using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Faturacao;

[Route("client/faturacao/credenciais-sns")]
[ApiController]
public class CredenciaisSnsController(ICredenciaisSnsService service) : ControllerBase
{
    private readonly ICredenciaisSnsService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost("{modulo}/paginated")]
    public async Task<IActionResult> GetPaginated(string modulo, [FromBody] CredenciaisSnsTableFilter filter)
    {
        filter.Modulo = modulo;
        return Ok(await _service.GetPaginatedAsync(filter));
    }

    [Authorize(Roles = "client")]
    [HttpDelete("{modulo}")]
    public async Task<IActionResult> Delete(
        string modulo,
        [FromBody] DeleteCredenciaisSnsRequest request,
        CancellationToken cancellationToken)
        => Ok(await _service.DeleteAsync(modulo, request, cancellationToken));
}
