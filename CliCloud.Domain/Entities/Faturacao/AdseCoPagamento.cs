#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Domain.Entities.Faturacao;

/// <summary>Equivalente legado: <c>dbo.ADSE_CoPagamentos</c>.</summary>
[Table("AdseCoPagamento", Schema = "Faturacao")]
public class AdseCoPagamento : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    /// <summary>Legado: c_tFatura — ligação principal à fatura.</summary>
    public Guid DocumentoId { get; set; }
    public Documento Documento { get; set; } = null!;

    [StringLength(2)]
    public string? TipoPreFatura { get; set; }

    /// <summary>Legado: N_Ordem (pré-fatura atribuída).</summary>
    public int? NumOrdemPreFatura { get; set; }

    /// <summary>Legado: Estado (1–4). Ver <see cref="AdseEstados"/>.</summary>
    public int Estado { get; set; }

    public DateTime? DataComunicacao { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotalUtente { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotalAdse { get; set; }

    [StringLength(260)]
    public string? PdfFicheiro { get; set; }

    [StringLength(260)]
    public string? PdfRelatorioFicheiro { get; set; }

    public string? Erros { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public int NumDevolucoes { get; set; }

    /// <summary>TratamentoId ou AdmissaoId conforme módulo.</summary>
    public Guid OrigemClinicaId { get; set; }

    [StringLength(50)]
    public string? NumeroDevolucao { get; set; }
}