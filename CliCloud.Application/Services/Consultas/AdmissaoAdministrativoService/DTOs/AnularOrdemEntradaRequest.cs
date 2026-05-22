using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class AnularOrdemEntradaRequest : IDto 
{
    public string? Motivo { get; set; }

}
public class AnularOrdemEntradaRequestValidator : AbstractValidator<AnularOrdemEntradaRequest>
{
    public AnularOrdemEntradaRequestValidator()
    {
        _ = RuleFor(x => x.Motivo)
            .NotEmpty()
            .WithMessage("Indique o motivo da anulação");
    }
}