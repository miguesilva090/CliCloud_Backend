using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliCloud.WebApi.Controllers.Consultas;

[Route("client/consultas/pedidos-consulta-administrativo")]
[ApiController]
public class PedidosConsultaAdministrativoController(IPedidosConsultaAdministrativoService service)
  : ControllerBase
{
  private readonly IPedidosConsultaAdministrativoService _service = service;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetPaginated([FromBody] PedidoConsultaTableFilter filter)
    => Ok(await _service.GetPaginatedAsync(filter));

  [Authorize(Roles = "client")]
  [HttpGet("{codigo:int}")]
  public async Task<IActionResult> GetById(int codigo)
    => Ok(await _service.GetByIdAsync(codigo));

  [Authorize(Roles = "client")]
  [HttpPost("{codigo:int}/recusado")]
  public async Task<IActionResult> SetRecusado(
    int codigo,
    [FromBody] SetPedidoConsultaRecusadoRequest request
  )
    => Ok(await _service.SetRecusadoAsync(codigo, request));

  [Authorize(Roles = "client")]
  [HttpDelete("{codigo:int}")]
  public async Task<IActionResult> Delete(int codigo)
    => Ok(await _service.DeleteAsync(codigo));

  [Authorize(Roles = "client")]
  [HttpPost("delete-multiple")]
  public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<int> codigos)
    => Ok(await _service.DeleteMultipleAsync(codigos));

  [Authorize(Roles = "client")]
  [HttpGet("{codigo:int}/ficheiro")]
  public async Task<IActionResult> DownloadFicheiro(int codigo)
    => Ok(await _service.DownloadFicheiroAsync(codigo));

  [Authorize(Roles = "client")]
  [HttpGet("{codigo:int}/utentes-candidatos")]
  public async Task<IActionResult> PesquisarUtentes(int codigo)
    => Ok(await _service.PesquisarUtentesAsync(codigo));

  [Authorize(Roles = "client")]
  [HttpPost("{codigo:int}/criar-utente")]
  public async Task<IActionResult> CriarUtente(int codigo, [FromQuery] bool forcar = false)
    => Ok(await _service.CriarUtenteFromPedidoAsync(codigo, forcar));

  [Authorize(Roles = "client")]
  [HttpPost("{codigo:int}/guardar-marcacao")]
  public async Task<IActionResult> GuardarMarcacao(
    int codigo,
    [FromBody] GuardarPedidoConsultaMarcacaoRequest request
  )
    => Ok(await _service.GuardarMarcacaoAsync(codigo, request));

  [Authorize(Roles = "client")]
  [HttpPost("{codigo:int}/enviar-email")]
  public async Task<IActionResult> EnviarEmail(int codigo, [FromQuery] int tipo)
    => Ok(await _service.EnviarEmailAsync(codigo, tipo));

  [Authorize(Roles = "client")]
  [HttpPost("{codigo:int}/enviar-sms")]
  public async Task<IActionResult> EnviarSms(int codigo, [FromQuery] int tipo)
    => Ok(await _service.EnviarSmsAsync(codigo, tipo));
}
