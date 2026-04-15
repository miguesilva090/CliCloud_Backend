using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class EmailAutomaticoEventoDTO : IDto
{
    public string CodigoRegra { get; set; } = string.Empty;
    public Guid ClinicaId { get; set; }
    public string EmailDestino { get; set; } = string.Empty;
    public string Assunto { get; set; } = string.Empty;
    public string MensagemTemplate { get; set; } = string.Empty;
    public Dictionary<string, string> Placeholders { get; set; } = [];
    public string Modulo { get; set; } = "EmailAutomatico";
}
