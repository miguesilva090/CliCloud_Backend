using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;

public class ObterTokenAssinadoRequest : IDto
{
    public Guid MedicoId { get; set; }
    public string DigestValue { get; set; } = string.Empty;
    public string SignatureValue { get; set; } = string.Empty;
    public string Assinatura { get; set; } = string.Empty;
    public string AssinaturaSubCa { get; set; } = string.Empty;
}
