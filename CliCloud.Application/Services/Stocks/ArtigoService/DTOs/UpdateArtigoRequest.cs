using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.ArtigoService.DTOs;

public class UpdateArtigoRequest : CreateArtigoRequest { }

public class UpdateArtigoValidator : AbstractValidator<UpdateArtigoRequest>
{
    public UpdateArtigoValidator()
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