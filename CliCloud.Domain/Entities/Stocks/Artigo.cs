using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.TaxasIva;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Stocks;

[Table("Artigo", Schema = "Stocks")]
public class Artigo : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    public Guid ClinicaId { get; set; }

    public int Codigo { get; set; }

    [Required]
    [StringLength(20)]
    public string NumeroArtigo { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Descricao { get; set; } = string.Empty;

    [StringLength(13)]
    public string? EAN { get; set; }

    [StringLength(50)]
    public string? CodigoBarras { get; set; }

    [StringLength(512)]
    public string? UrlFoto { get; set; }

    public Guid UnidadeMedidaId { get; set; }
    public UnidadeMedida UnidadeMedida { get; set; } = null!;

    public Guid? FamiliaArtigoId { get; set; }
    public FamiliaArtigo? FamiliaArtigo { get; set; }

    public Guid TaxaIvaId { get; set; }
    public TaxaIva TaxaIva { get; set; } = null!;

    public Guid? MotivoIsencaoId { get; set; }
    public MotivoIsencao? MotivoIsencao { get; set; }

    public Guid ArmazemId { get; set; }
    public Armazem Armazem { get; set; } = null!;

    public TipoArtigoStocks TipoArtigo { get; set; } = TipoArtigoStocks.Artigo;

    public bool Inativo { get; set; }
    public bool Descontinuado { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoUnitarioSemIva1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoUnitarioSemIva2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoUnitarioSemIva3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoVendaComIva1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoVendaComIva2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoVendaComIva3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoCusto { get; set; }

    /// <summary>Último preço de compra (agregado; actualizado por movimentos de stock).</summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal UltimoPrecoFinal { get; set; }

    /// <summary>Preço médio de compra (agregado; actualizado por movimentos de stock).</summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoMedioFinal { get; set; }

    /// <summary>Último preço de venda (agregado; actualizado por movimentos de stock).</summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal UltimoPrecoVenda { get; set; }

    /// <summary>Preço médio de venda (agregado; actualizado por movimentos de stock).</summary>
    [Column(TypeName = "decimal(18, 4)")]
    public decimal PrecoMedioVenda { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? StockMinimo { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? StockMaximo { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? StockReposicao { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal StockReal { get; set; }

    public bool PermitirDescontos { get; set; } = true;
    public bool PermitirAlterarPreco { get; set; } = true;
    public bool ActHotel { get; set; }
    public bool ActPOS { get; set; }

    [StringLength(100)]
    public string? NumSerieUCentral { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? Desconto { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? Capacidade { get; set; }

    public bool TemGarantia { get; set; }
    public int? MesesGarantia { get; set; }
    public int? AmpliacaoGarantia { get; set; }
    public bool VisualizarNaNet { get; set; }
    public TipoMedidaArtigo? TipoMedida { get; set; }

    public int? CodigoInternoLegado { get; set; }

}