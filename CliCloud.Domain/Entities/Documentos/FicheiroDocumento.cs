using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Documentos;

public class FicheiroDocumento : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    public Guid InstanciaDocumentoId { get; set; }
    public InstanciaDocumento? InstanciaDocumento { get; set; }

    public string NomeOriginal { get; set; } = string.Empty;
    public string NomeArmazenamento { get; set; } = string.Empty;
    public string CaminhoRelativo { get; set; } = string.Empty;
    public string TipoMime { get; set; } = "application/octet-stream";
    public long TamanhoBytes { get; set; }

    public string? ChecksumSha256 { get; set; }
}