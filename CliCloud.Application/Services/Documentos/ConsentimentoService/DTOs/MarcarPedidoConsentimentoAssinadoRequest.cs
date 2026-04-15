namespace CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;

public class MarcarPedidoConsentimentoAssinadoRequest 
{
    public string? AssinadoPor { get; set; }
    public string? Observacoes { get; set; }
    public string? AssinaturaBase64 { get; set; }
}