using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.ZonaComercialService.DTOs;

public class CreateZonaComercialRequest : IDto
{
    public required string Descricao { get; set; }

}

public class CreateZonaComercialValidator : AbstractValidator<CreateZonaComercialRequest>
{
    public CreateZonaComercialValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(40);
    }
}