using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;

public class UpdateSubsistemaArtigoRequest : IDto 
{
    public string CodigoCartaoInstituicao { get; set; } = string.Empty;
    public decimal ValorServico { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal ValorUtente { get; set; }
    public bool Inativo { get; set; }
    public string? CodigoComplementarAdse { get; set; }
}

public class UpdateSubsistemaArtigoValidator : AbstractValidator<UpdateSubsistemaArtigoRequest>
{

    public UpdateSubsistemaArtigoValidator()
    {
        _ = RuleFor(x => x.CodigoCartaoInstituicao).NotEmpty().MaximumLength(20);
        _ = RuleFor(x => x.CodigoComplementarAdse).MaximumLength(10);
        _ = RuleFor(x => x.ValorServico).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.ValorOrganismo).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.ValorUtente).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.MargemOrganismoPercent).InclusiveBetween(0, 100);
    }
}