using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/admissoes-administrativo")]
[ApiController]
public class AdmissaoAdministrativoController(IAdmissaoAdministrativoService service) : ControllerBase
{
  private readonly IAdmissaoAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] AdmissaoTableFilter filter)
    => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetById(Guid id)
    => Ok(await _service.GetByIdAsync(id));

  [Authorize(Roles = "client")]
  [HttpGet("por-marcacao/{consultaMarcacaoId:guid}")]
  public async Task<IActionResult> GetByConsultaMarcacaoId(Guid consultaMarcacaoId)
    => Ok(await _service.GetByConsultaMarcacaoIdAsync(consultaMarcacaoId));

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateAdmissaoRequest request)
    => Ok(await _service.CreateAsync(request));

  [Authorize(Roles = "client")]
  [HttpPut("{id:guid}")]
  public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAdmissaoRequest request)
    => Ok(await _service.UpdateAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> Delete(Guid id)
    => Ok(await _service.DeleteAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/confirmar")]
  public async Task<IActionResult> Confirmar(Guid id, [FromBody] bool confirmado)
    => Ok(await _service.ConfirmarAsync(id, confirmado));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/confirma-consulta")]
  public async Task<IActionResult> SetConfirmaConsulta(Guid id, [FromBody] bool confirmaConsulta)
    => Ok(await _service.SetConfirmaConsultaAsync(id, confirmaConsulta));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/em-tratamento")]
  public async Task<IActionResult> SetEmTratamento(Guid id, [FromBody] bool emTratamento)
    => Ok(await _service.SetEmTratamentoAsync(id, emTratamento));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/efetuado")]
  public async Task<IActionResult> SetEfetuado(Guid id, [FromBody] bool efetuado)
    => Ok(await _service.SetEfetuadoAsync(id, efetuado));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/desmarcar")]
  public async Task<IActionResult> Desmarcar(Guid id, [FromBody] DesmarcarAdmissaoRequest request)
    => Ok(await _service.DesmarcarAsync(id, request));

  [Authorize(Roles = "client")]
  [HttpPost("promover-lote")]
  public async Task<IActionResult> PromoverLote([FromBody] PromoverAdmissaoLoteRequest request)
    => Ok(await _service.PromoverLoteAsync(request));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/promover-consulta")]
  public async Task<IActionResult> PromoverParaConsulta(Guid id)
    => Ok(await _service.PromoverParaConsultaAsync(id));

  [Authorize(Roles = "client")]
  [HttpGet("{id:guid}/observacoes")]
  public async Task<IActionResult> GetObservacoes(Guid id)
    => Ok(await _service.GetObservacoesAsync(id));

  [Authorize(Roles = "client")]
  [HttpPost("{id:guid}/observacoes")]
  public async Task<IActionResult> AppendObservacao(
    Guid id,
    [FromBody] AppendAdmissaoObservacaoRequest request
  )
    => Ok(await _service.AppendObservacaoAsync(id, request));

}