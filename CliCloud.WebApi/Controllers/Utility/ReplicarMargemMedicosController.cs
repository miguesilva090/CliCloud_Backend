using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Services.Utility.ReplicarMargemMedicosService;
using CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.DTOs;

namespace CliCloud.WebApi.Controllers.Utility;

[Route("client/utility/replicar-margem-medicos")]
[ApiController]
public class ReplicarMargemMedicosController(IReplicarMargemMedicosService service) : ControllerBase
{
    private readonly IReplicarMargemMedicosService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost]
    public async Task<IActionResult> ReplicarAsync([FromBody] ReplicarMargemMedicosRequest request)
    {
        var result = await _service.ReplicarAsync(request);
        return Ok(result);
    }
}
