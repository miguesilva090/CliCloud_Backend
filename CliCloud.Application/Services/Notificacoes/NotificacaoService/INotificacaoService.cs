using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoService.Filters;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService;

public interface INotificacaoService : ITransientService
{
  Task<PaginatedResponse<NotificacaoTableDTO>> GetNotificacaoPaginatedAsync(NotificacaoTableFilter filter);
  Task<Response<IEnumerable<NotificacaoTableDTO>>> GetAllNotificacaoAsync(NotificacaoAllFilter filter);
  Task<Response<NotificacaoDTO>> GetNotificacaoAsync(Guid id);
  Task<Response<Guid>> CreateNotificacaoAsync(CreateNotificacaoRequest request);
  Task<Response<Guid>> MarcarComoLidaAsync(Guid id);
  Task<Response<Guid>> DeleteNotificacaoAsync(Guid id);
}
