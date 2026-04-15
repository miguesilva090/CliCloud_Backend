using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;

public class InstanciaDocumentoDTO: IDto
{
    public Guid Id { get; set; }
    public Guid ModeloDocumentoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ConteudoHtml { get; set; } = string.Empty;
    public Guid? UtenteId { get; set; }
    public bool Assinado { get; set; }
    public DateTime CreatedOn { get; set; }
    
}