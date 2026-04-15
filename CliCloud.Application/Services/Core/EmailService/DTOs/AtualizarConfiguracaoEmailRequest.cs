using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class AtualizarConfiguracaoEmailRequest : IDto 
{
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

public class EnviarEmailPorCodigoRequest : IDto
{
    public string CodigoConfiguracao { get; set; } = string.Empty;
    public string EmailDestino { get; set; } = string.Empty;
    public string NomeUtente { get; set; } = string.Empty;
    public string? NomeMedicoOuProfissional { get; set; }
    public string? NomeEspecialidade { get; set; }
    public string? NumeroSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? Hora { get; set; }
    public string? HoraAntiga { get; set; }
    public string? DataAntiga { get; set; }
    public string? HoraNova { get; set; }
    public string? DataNova { get; set; }
    public string Modulo { get; set; } = "EmailAutomatico";
}