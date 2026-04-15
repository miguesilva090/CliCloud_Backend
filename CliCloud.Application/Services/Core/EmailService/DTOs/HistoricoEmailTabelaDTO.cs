using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class HistoricoEmailTabelaDTO : IDto
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public string? AssuntoEmail { get; set; }
    public string CorpoEmail { get; set; } = string.Empty;
    public string? EmailDestino { get; set; }
    public string? NomeUtente { get; set; }
    public string? Contacto { get; set; }
    public DateTime DataHoraCriacao { get; set; }
    public DateTime? DataHoraEnvio { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? MensagemErro { get; set; }
    public string Modulo { get; set; } = string.Empty;
}
