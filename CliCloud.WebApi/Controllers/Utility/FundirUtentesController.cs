using CliCloud.Application.Services.Utility.FundirUtentesService;
using CliCloud.Application.Services.Utility.FundirUtentesService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Utility;

[Route("client/utility/fundir-utentes")]
[ApiController]
public class FundirUtentesController(IFundirUtentesService service) : ControllerBase
{
    private readonly IFundirUtentesService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> FundirUtentesAsync(
        [FromBody] FundirUtentesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.FundirUtentesAsync(request, cancellationToken);
        return Ok(result);
    }
}
