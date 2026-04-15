using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;

public class FicheiroDocumentoDTO 
{
    public Guid Id { get; set; }
    public Guid InstanciaDocumentoId { get; set; }
    public string NomeOriginal { get; set; } = string.Empty;
    public string NomeArmazenamento { get; set; } = string.Empty;
    public string CaminhoRelativo { get; set; } = string.Empty;
    public string TipoMime { get; set; } = "application/octet-stream";
    public long TamanhoBytes { get; set; }
    public string? ChecksumSha256 { get; set; }
    public DateTime CreatedOn { get; set; }
}