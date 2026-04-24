using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Filters;

namespace CliCloud.WebApi.Controllers.Notificacoes;

[Route("client/notificacoes/tipos")]
[ApiController]
public class NotificacaoTipoController(INotificacaoTipoService notificacaoTipoService) : ControllerBase
{
  private readonly INotificacaoTipoService _notificacaoTipoService = notificacaoTipoService;

  [Authorize(Roles = "client")]
  [HttpGet]
  public async Task<IActionResult> GetNotificacaoTipoAsync(string keyword = "")
  {
    Response<IEnumerable<NotificacaoTipoDTO>> result = await _notificacaoTipoService.GetNotificacaoTipoAsync(keyword);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpGet("light")]
  public async Task<IActionResult> GetNotificacaoTipoLightAsync(string keyword = "")
  {
    Response<IEnumerable<NotificacaoTipoLightDTO>> result =
      await _notificacaoTipoService.GetNotificacaoTipoLightAsync(keyword);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpPost("paginated")]
  public async Task<IActionResult> GetNotificacaoTipoPaginatedAsync(NotificacaoTipoTableFilter filter)
  {
    PaginatedResponse<NotificacaoTipoTableDTO> result =
      await _notificacaoTipoService.GetNotificacaoTipoPaginatedAsync(filter);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpPost("all")]
  public async Task<IActionResult> GetAllNotificacaoTipoAsync([FromBody] NotificacaoTipoAllFilter filter)
  {
    try
    {
      Response<IEnumerable<NotificacaoTipoTableDTO>> result =
        await _notificacaoTipoService.GetAllNotificacaoTipoAsync(filter);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpGet("{id}")]
  public async Task<IActionResult> GetNotificacaoTipoAsync(Guid id)
  {
    Response<NotificacaoTipoDTO> result = await _notificacaoTipoService.GetNotificacaoTipoAsync(id);
    return Ok(result);
  }

  [Authorize(Roles = "client")]
  [HttpPost]
  public async Task<IActionResult> CreateNotificacaoTipoAsync(CreateNotificacaoTipoRequest request)
  {
    try
    {
      Response<Guid> result = await _notificacaoTipoService.CreateNotificacaoTipoAsync(request);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateNotificacaoTipoAsync([FromRoute] Guid id, [FromBody] UpdateNotificacaoTipoRequest request)
  {
    try
    {
      Response<Guid> result = await _notificacaoTipoService.UpdateNotificacaoTipoAsync(request, id);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteNotificacaoTipoAsync(Guid id)
  {
    try
    {
      Response<Guid> response = await _notificacaoTipoService.DeleteNotificacaoTipoAsync(id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [Authorize(Roles = "client")]
  [HttpDelete("bulk")]
  public async Task<IActionResult> DeleteMultipleNotificacaoTipoAsync([FromBody] DeleteMultipleNotificacaoTipoRequest request)
  {
    try
    {
      Response<IEnumerable<Guid>> result =
        await _notificacaoTipoService.DeleteMultipleNotificacaoTipoAsync(request.Ids);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
