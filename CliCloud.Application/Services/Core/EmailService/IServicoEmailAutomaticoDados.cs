using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Core.EmailService.DTOs;

namespace CliCloud.Application.Services.Core.EmailService;

public interface IServicoEmailAutomaticoDados : ITransientService
{
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosConsultasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosPrimeiraSessaoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosSessoesAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosSessoesFinaisAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosAniversariosAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosPedidoAgendamentoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosPedidoAgendamentoConfirmadoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<EmailAutomaticoEventoDTO>> ObterEventosAulasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
}
