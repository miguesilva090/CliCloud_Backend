#nullable enable

using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs;

public sealed class DocumentoPrintDTO : IDto 
{
    public string Template { get; set; } = "TFatura";
    public bool IsAnulado { get; set; }
    public bool IsEmitido { get; set; }
    public string TipoSerie { get; set; } = string.Empty;
    public string NumeroExibicao { get; set; } = string.Empty;
}