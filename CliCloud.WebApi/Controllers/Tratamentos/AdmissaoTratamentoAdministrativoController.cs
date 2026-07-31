using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Tratamentos;

[Route("client/tratamentos/admissao-tratamento-administrativo")]
[ApiController]
public class AdmissaoTratamentoAdministrativoController(
  IAdmissaoTratamentoAdministrativoService service
) : ControllerBase
{
  private readonly IAdmissaoTratamentoAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated(
    [FromBody] AdmissaoTratamentoTableFilter filter
  ) => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}/situacao")]
  public async Task<IActionResult> UpdateSituacao(
    Guid id,
    [FromBody] UpdateAdmissaoTratamentoSituacaoRequest request
  ) => Ok(await _service.UpdateSituacaoAsync(id, request));
}
