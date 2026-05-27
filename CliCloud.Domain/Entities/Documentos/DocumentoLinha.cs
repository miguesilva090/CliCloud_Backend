#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Documentos;

/// <summary>
/// Linha de um documento fiscal (equivalente legado <c>Faturacao.TFaturaLinha</c>).
/// </summary>
[Table("DocumentoLinha", Schema = "Documentos")]
public class DocumentoLinha : AuditableEntityWithSoftDelete
{
  public Guid DocumentoId { get; set; }
  public Documento? Documento { get; set; }

  /// <summary>Ordem da linha no documento (1-based).</summary>
  public int NumeroLinha { get; set; }

  /// <summary>Código de artigo legado / stocks (texto até existir entidade Artigo).</summary>
  [StringLength(50)]
  public string? CodigoArtigo { get; set; }

  public Guid? ServicoId { get; set; }
  public Servico? Servico { get; set; }

  public Guid? AdmissaoServicoId { get; set; }
  public AdmissaoServico? AdmissaoServico { get; set; }

  [Required]
  [StringLength(250)]
  public string Descricao { get; set; } = string.Empty;

  [Column(TypeName = "decimal(18,4)")]
  public decimal Quantidade { get; set; }

  [Column(TypeName = "decimal(18,4)")]
  public decimal PrecoUnitario { get; set; }

  [Column(TypeName = "decimal(18,4)")]
  public decimal? PercentagemDesconto { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? ValorDesconto { get; set; }

  [Column(TypeName = "decimal(18,4)")]
  public decimal? DescontoTipo1 { get; set; }

  [Column(TypeName = "decimal(18,4)")]
  public decimal? DescontoTipo2 { get; set; }

  [Column(TypeName = "decimal(18,4)")]
  public decimal? DescontoTipo3 { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? TotalLinha { get; set; }

  public Guid? TaxaIvaId { get; set; }
  public TaxaIva? TaxaIva { get; set; }

  /// <summary>Taxa aplicada no momento da emissão (snapshot fiscal).</summary>
  [Column(TypeName = "decimal(18,2)")]
  public decimal TaxaIvaPercentagem { get; set; }

  [Column(TypeName = "decimal(18,2)")]
  public decimal? ValorImposto { get; set; }

  public ModuloOrigemDocumento? ModuloOrigemLinha { get; set; }
}
