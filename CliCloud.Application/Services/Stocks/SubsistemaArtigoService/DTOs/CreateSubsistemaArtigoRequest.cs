using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.SubsistemaArtigoService.DTOs;

public class CreateSubsistemaArtigoRequest : IDto 
{
    public Guid ArtigoId { get; set; }
    public Guid OrganismoId { get; set; }
    public string CodigoCartaoInstituicao { get; set; } = string.Empty;
    public decimal ValorServico { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal ValorUtente { get; set; }
    public bool Inativo { get; set; }
    public string? CodigoComplementarAdse { get; set; }
}

public class CreateSubsistemaArtigoValidator : AbstractValidator<CreateSubsistemaArtigoRequest>
{
    public CreateSubsistemaArtigoValidator()
    {
        _ = RuleFor(x => x.ArtigoId).NotEmpty();
        _ = RuleFor(x => x.OrganismoId).NotEmpty();
        _ = RuleFor(x => x.CodigoCartaoInstituicao).MaximumLength(20);
        _ = RuleFor(x => x.CodigoComplementarAdse).MaximumLength(10);
        _ = RuleFor(x => x.ValorServico).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.ValorOrganismo).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.ValorUtente).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.MargemOrganismoPercent).InclusiveBetween(0, 100);
    }
}