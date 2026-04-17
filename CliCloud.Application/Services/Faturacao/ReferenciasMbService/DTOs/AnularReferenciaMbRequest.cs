using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class AnularReferenciaMbRequest : IDto 
{
    public string Observacao { get; set; } = string.Empty;

}

public class AnularReferenciaMbValidator : AbstractValidator<AnularReferenciaMbRequest>
{
    public AnularReferenciaMbValidator()
    {
    _ =  RuleFor(x => x.Observacao)
        .NotEmpty()
        .MaximumLength(2000);
    }
}