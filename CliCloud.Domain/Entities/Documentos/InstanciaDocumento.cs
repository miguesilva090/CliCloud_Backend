using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Documentos;

public class InstanciaDocumento : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    public Guid ModeloDocumentoId { get; set; }

    public ModeloDocumento? ModeloDocumento { get; set; }

    public int VersaoModelo { get; set; } 

    public Guid? UtenteId { get; set; }
    
    public string Titulo { get; set; } = string.Empty;
    public string ConteudoHtml { get; set; } = string.Empty;
    public bool Assinado { get; set; } 
    public DateTime? AssinadoEm { get; set; }
    public string? AssinadoPor { get; set; }
    
}