using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.ReplicarSubsistemasService;
using CliCloud.Application.Services.Utility.ReplicarSubsistemasService.DTOs;

namespace CliCloud.WebApi.Controllers.Utility;

[Route("client/utility/replicar-subsistemas")]
[ApiController]
public class ReplicarSubsistemasController(IReplicarSubsistemasService service) : ControllerBase
{
    private readonly IReplicarSubsistemasService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> ReplicarSubsistemasAsync([FromBody] ReplicarSubsistemasRequest request)
    {
        var result = await _service.ReplicarSubsistemasAsync(request);
        return Ok(result);
    }
}
