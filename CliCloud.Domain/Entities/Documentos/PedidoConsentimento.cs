using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Domain.Entities.Documentos;

public class PedidoConsentimento : AuditableEntity
{
    public Guid ClinicaId { get; set; }
    public Guid InstanciaDocumentoId { get; set; }
    public Guid? UtenteId { get; set; }

    public EstadoPedidoConsentimento Estado { get; set; } = EstadoPedidoConsentimento.Pendente;
    public string TipoConsentimento { get; set; } = "RGPD";

    public string? Canal { get; set; } 
    public DateTime? ExpiraEm { get; set; }
    
    public DateTime? AssinadoEm { get; set; }
    public string? AssinadoPor { get; set; }
    public string? Observacoes { get; set; }

    public InstanciaDocumento? InstanciaDocumento { get; set; }

}