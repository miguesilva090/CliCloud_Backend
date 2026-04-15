namespace CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;

public class CriarPedidoConsentimentoRequest 
{
    public Guid InstanciaDocumentoId { get; set; }
    public Guid? UtenteId { get; set; }
    public string TipoConsentimento { get; set; } = "RGPD";
    public string? Canal { get; set; }
    public DateTime? ExpiraEm { get; set; }
}