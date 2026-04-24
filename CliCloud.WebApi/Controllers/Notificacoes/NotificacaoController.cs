using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Notificacoes.NotificacaoService;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;

namespace CliCloud.WebApi.Controllers.Notificacoes;

[Route("client/notificacoes")]
[ApiController]
public class NotificacaoController(INotificacaoService notificacaoService) : ControllerBase
{
  private readonly INotificacaoService _notificacaoService = notificacaoService;

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetNotificacaoPaginatedAsync(NotificacaoTableFilter filter)
  {
    PaginatedResponse<NotificacaoTableDTO> result = await _notificacaoService.GetNotificacaoPaginatedAsync(filter);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpPost("all")]
  public async Task<IActionResult> GetAllNotificacaoAsync([FromBody] NotificacaoAllFilter filter)
  {
    try
    {
      Response<IEnumerable<NotificacaoTableDTO>> result = await _notificacaoService.GetAllNotificacaoAsync(filter);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpGet("{id}")]
  public async Task<IActionResult> GetNotificacaoAsync(Guid id)
  {
    Response<NotificacaoDTO> result = await _notificacaoService.GetNotificacaoAsync(id);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> CreateNotificacaoAsync(CreateNotificacaoRequest request)
  {
    try
    {
      Response<Guid> result = await _notificacaoService.CreateNotificacaoAsync(request);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpPut("{id}/lida")]
  public async Task<IActionResult> MarcarComoLidaAsync(Guid id)
  {
    try
    {
      Response<Guid> result = await _notificacaoService.MarcarComoLidaAsync(id);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteNotificacaoAsync(Guid id)
  {
    try
    {
      Response<Guid> response = await _notificacaoService.DeleteNotificacaoAsync(id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
