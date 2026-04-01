using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Core.SmsService.DTOs;

namespace CliCloud.Application.Services.Core.SmsService;

public interface IServicoSmsAutomaticoDados : ITransientService
{
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosConsultasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosPrimeiraSessaoAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosSessoesAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosSessoesFinaisAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosAniversariosAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    Task<List<SmsAutomaticoEventoDTO>> ObterEventosAulasAsync(Guid clinicaId, DateTime dataAlvo, string templateMensagem, CancellationToken ct);
    
}