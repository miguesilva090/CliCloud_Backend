using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class ArtigoDTO : IDto 
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string NumeroArtigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string? EAN { get; set; }
    public string? CodigoBarras { get; set; }
    public string? UrlFoto { get; set; }

    public Guid UnidadeMedidaId { get; set; }
    public string? UnidadeMedidaDescricao { get; set; }
    public Guid? FamiliaArtigoId { get; set; }
    public string? FamiliaArtigoDescricao { get; set; }
    public Guid TaxaIvaId { get; set; }
    public string? TaxaIvaDescricao { get; set; }
    public decimal? TaxaIvaPercentagem { get; set; }

    public Guid? MotivoIsencaoId { get; set; }
    public string? MotivoIsencaoDescricao { get; set; }

    public Guid ArmazemId { get; set; }
    public string? ArmazemNome { get; set; }

    public TipoArtigoStocks TipoArtigo { get; set; }
    public bool Inativo { get; set; }
    public bool Descontinuado { get; set; }
    
    public decimal PrecoUnitarioSemIva1 { get; set; }
    public decimal PrecoUnitarioSemIva2 { get; set; }
    public decimal PrecoUnitarioSemIva3 { get; set; }
    public decimal PrecoVendaComIva1 { get; set; }
    public decimal PrecoVendaComIva2 { get; set; }
    public decimal PrecoVendaComIva3 { get; set; }
    public decimal PrecoCusto { get; set; }

    public decimal UltimoPrecoFinal { get; set; }
    public decimal PrecoMedioFinal { get; set; }
    public decimal UltimoPrecoVenda { get; set; }
    public decimal PrecoMedioVenda { get; set; }

    public decimal? StockMinimo { get; set; }
    public decimal? StockMaximo { get; set; }
    public decimal? StockReposicao { get; set; }
    public decimal StockReal { get; set; }

    public bool PermitirDescontos { get; set; }
    public bool PermitirAlterarPreco { get; set; }
    public bool ActHotel { get; set; }
    public bool ActPOS { get; set; }

    public DateTime CreatedOn { get; set; } 
    public DateTime? LastModifiedOn { get; set; }

}