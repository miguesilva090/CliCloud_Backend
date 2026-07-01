#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Domain.Entities.Faturacao;

[Table("AdsePreFatura", Schema = "Faturacao")]
public class AdsePreFatura : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    [StringLength(2)]
    public string TipoPreFatura { get; set; } = AdseEstados.TipoTratamentos;

    /// <summary>Legado: N_Ordem.</summary>
    public int NumOrdem { get; set; }

    public int Estado { get; set; }

    public DateTime DataAbertura { get; set; }
    public DateTime? DataFecho { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal { get; set; }

    public int NumDocumentos { get; set; }

    [StringLength(260)]
    public string? PdfFicheiro { get; set; }

    public Guid? DocumentoFechoId { get; set; }
    public Documento? DocumentoFecho { get; set; }

    [StringLength(50)]
    public string? ReferenciaSerie { get; set; }
    public int? ReferenciaNumeroDocumento { get; set; }
    public DateTime? ReferenciaData { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ReferenciaValor { get; set; }
    
}