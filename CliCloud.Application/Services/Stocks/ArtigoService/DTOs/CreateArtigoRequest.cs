using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class CreateArtigoRequest : IDto 
{
    public string? NumeroArtigo { get; set; }
    public required string Descricao { get; set; }
    public string? EAN { get; set; }
    public string? CodigoBarras { get; set; }
    public string? UrlFoto { get; set; }

    public Guid UnidadeMedidaId { get; set; }
    public Guid? FamiliaArtigoId { get; set; }
    public Guid TaxaIvaId { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
    public Guid ArmazemId { get; set; }

    public TipoArtigoStocks TipoArtigo { get; set; } = TipoArtigoStocks.Artigo;

    public bool Inativo { get; set; }

    public bool Descontinuado { get; set; }

    public decimal PrecoUnitarioSemIva1 { get; set; }
    public decimal PrecoUnitarioSemIva2 { get; set; }
    public decimal PrecoUnitarioSemIva3 { get; set; }
    public decimal PrecoVendaComIva1 { get; set; }
    public decimal PrecoVendaComIva2 { get; set; }
    public decimal PrecoVendaComIva3 { get; set; }
    public decimal PrecoCusto { get; set; }

    public decimal? StockMinimo { get; set; }
    public decimal? StockMaximo { get; set; }
    public decimal? StockReposicao { get; set; }

    public bool PermitirDescontos { get; set; } = true;
    public bool PermitirAlterarPreco { get; set; } = true;
    public bool ActHotel { get; set; }
    public bool ActPOS { get; set; }

    public string? NumSerieUCentral { get; set; }
    public decimal? Desconto { get; set; }
    public decimal? Capacidade { get; set; }
    public bool TemGarantia { get; set; }
    public int? MesesGarantia { get; set; }
    public int? AmpliacaoGarantia { get; set; }
    public bool VisualizarNaNet { get; set; }
    public TipoMedidaArtigo? TipoMedida { get; set; }

}

public class CreateArtigoValidator : AbstractValidator<CreateArtigoRequest>
{
    public CreateArtigoValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(100);
        _ = RuleFor(x => x.NumeroArtigo).MaximumLength(20);
        _ = RuleFor(x => x.EAN).MaximumLength(13);
        _ = RuleFor(x => x.CodigoBarras).MaximumLength(50);
        _ = RuleFor(x => x.UrlFoto).MaximumLength(512);
        _ = RuleFor(x => x.NumSerieUCentral).MaximumLength(100);
        _ = RuleFor(x => x.UnidadeMedidaId).NotEmpty();
        _ = RuleFor(x => x.TaxaIvaId).NotEmpty();
        _ = RuleFor(x => x.ArmazemId).NotEmpty();
    }
}