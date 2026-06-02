#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public sealed class EnviarDocumentoEmailRequest : IDto 
{
    public string? DestinatarioOverride { get; set; }
    public string? AssuntoOverride { get; set; }
    public string? MensagemOverride { get; set; }
    
}