using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class ConfiguracaoEmailDTO : IDto
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Server { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Porta { get; set; }
    public bool UseSSL { get; set; }
    public int TipoServico { get; set; }

    public string? Inbox { get; set; }
    public string? Outbox { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public bool PermitirEliminarEmail { get; set; }
}