using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.ReplicarPatologiasService;
using CliCloud.Application.Services.Utility.ReplicarPatologiasService.DTOs;

namespace CliCloud.WebApi.Controllers.Utility;

[Route("client/utility/replicar-patologias")]
[ApiController]
public class ReplicarPatologiasController(IReplicarPatologiasService service) : ControllerBase
{
    private readonly IReplicarPatologiasService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> ReplicarPatologiasAsync([FromBody] ReplicarPatologiasRequest request)
    {
        var result = await _service.ReplicarPatologiasAsync(request);
        return Ok(result);
    }
}