using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Application.Services.Documentos.ConsentimentoService.DTOs;

public class PedidoConsentimentoDTO 
{
    public Guid Id { get; set; }
    public Guid InstanciaDocumentoId { get; set; }
    public Guid? UtenteId { get; set; }
    public string TipoConsentimento { get; set; } = string.Empty;
    public EstadoPedidoConsentimento Estado { get; set; }
    public string? Canal { get; set; }
    public DateTime? ExpiraEm { get; set; }
    public DateTime? AssinadoEm { get; set; }
    public string? AssinadoPor { get; set; }
    public string? Observacoes { get; set; }
    public DateTime CreatedOn { get; set; }
}