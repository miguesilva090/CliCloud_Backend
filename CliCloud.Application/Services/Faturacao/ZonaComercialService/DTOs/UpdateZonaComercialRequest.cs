using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;

public class UpdateZonaComercialRequest : IDto 
{
    public required string Descricao { get; set; }
}

public class UpdateZonaComercialValidator : AbstractValidator<UpdateZonaComercialRequest>
{
    public UpdateZonaComercialValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(40);
    }
}