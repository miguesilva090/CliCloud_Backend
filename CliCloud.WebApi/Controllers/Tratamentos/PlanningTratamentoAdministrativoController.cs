using CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/PlanningTratamentoAdministrativo")]
[ApiController]
public class PlanningTratamentoAdministrativoController(IPlanningTratamentoAdministrativoService service) : ControllerBase
{
    private readonly IPlanningTratamentoAdministrativoService _service = service;

    [Authorize(Roles = "client")]
    [HttpPost("sessoes")]
    public async Task<IActionResult> GetSessoes([FromBody] PlanningSessoesRequest request)
    => Ok(await _service.GetSessoesAsync(request));
}