using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Filters;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService;

public interface INotificacaoTipoService : ITransientService
{
  Task<Response<IEnumerable<NotificacaoTipoDTO>>> GetNotificacaoTipoAsync(string keyword = "");
  Task<Response<IEnumerable<NotificacaoTipoLightDTO>>> GetNotificacaoTipoLightAsync(string keyword = "");
  Task<PaginatedResponse<NotificacaoTipoTableDTO>> GetNotificacaoTipoPaginatedAsync(NotificacaoTipoTableFilter filter);
  Task<Response<IEnumerable<NotificacaoTipoTableDTO>>> GetAllNotificacaoTipoAsync(NotificacaoTipoAllFilter filter);
  Task<Response<NotificacaoTipoDTO>> GetNotificacaoTipoAsync(Guid id);
  Task<Response<Guid>> CreateNotificacaoTipoAsync(CreateNotificacaoTipoRequest request);
  Task<Response<Guid>> UpdateNotificacaoTipoAsync(UpdateNotificacaoTipoRequest request, Guid id);
  Task<Response<Guid>> DeleteNotificacaoTipoAsync(Guid id);
  Task<Response<IEnumerable<Guid>>> DeleteMultipleNotificacaoTipoAsync(IEnumerable<Guid> ids);
}
