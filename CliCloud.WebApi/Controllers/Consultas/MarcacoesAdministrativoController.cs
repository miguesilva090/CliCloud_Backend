using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/marcacoes-administrativo")]
[ApiController]
public class MarcacoesAdministrativoController(IMarcacoesAdministrativoService service)
  : ControllerBase
{
  private readonly IMarcacoesAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] MarcacaoAdministrativoTableFilter filter)
    => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpPost("calendario")]
  public async Task<IActionResult> GetCalendario([FromBody] MarcacaoCalendarioRequest request)
    => Ok(await _service.GetCalendarioAsync(request));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
    => Ok(await _service.GetByIdAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateMarcacaoAdministrativoRequest request)
    => Ok(await _service.CreateAsync(request));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(
    Guid id,
    [FromBody] UpdateMarcacaoAdministrativoRequest request
  )
    => Ok(await _service.UpdateAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/desmarcar")]
  public async Task<IActionResult> Desmarcar(
    Guid id,
    [FromBody] DesmarcarMarcacaoAdministrativoRequest request
  )
    => Ok(await _service.DesmarcarAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/mudar-horario")]
  public async Task<IActionResult> MudarHorario(
    Guid id,
    [FromBody] MudarHorarioMarcacaoAdministrativoRequest request
  )
    => Ok(await _service.MudarHorarioAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("salas-disponiveis")]
  public async Task<IActionResult> GetSalasDisponiveis([FromBody] SalasDisponiveisRequest request)
    => Ok(await _service.GetSalasDisponiveisAsync(request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/associar-sala")]
  public async Task<IActionResult> AssociarSala(
    Guid id,
    [FromBody] AssociarSalaMarcacaoRequest request
  )
    => Ok(await _service.AssociarSalaAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpDelete("{id:guid}/remover-sala")]
  public async Task<IActionResult> RemoverSala(Guid id)
    => Ok(await _service.RemoverSalaAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("troca-medicos/preview")]
  public async Task<IActionResult> PreviewTrocaMedicos(
    [FromBody] TrocaMarcacoesMedicosRequest request
  )
    => Ok(await _service.PreviewTrocaMedicosAsync(request));

  [Authorize(Roles = "client")]
  [HttpPost("troca-medicos/executar")]
  public async Task<IActionResult> ExecutarTrocaMedicos(
    [FromBody] TrocaMarcacoesMedicosRequest request
  )
    => Ok(await _service.ExecutarTrocaMedicosAsync(request));

  [Authorize(Roles = "client")]
  [HttpPost("disponibilidade-medicos-mes")]
  public async Task<IActionResult> GetDisponibilidadeMedicosMes(
    [FromBody] DisponibilidadeMedicosMesRequest request
  )
    => Ok(await _service.GetDisponibilidadeMedicosMesAsync(request));

  [Authorize(Roles = "client")]
  [HttpGet("resolve-medico-legado")]
  public async Task<IActionResult> ResolveMedicoLegado([FromQuery] string key)
    => Ok(await _service.ResolveMedicoLegadoAsync(key));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/sincronizar-admissao")]
  public async Task<IActionResult> SincronizarAdmissao(Guid id)
    => Ok(await _service.SincronizarAdmissaoAsync(id));
}
