using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class DefinirOrdemEntradaRequest : IDto
{
    public int Ordem { get; set; }
}

public class DefinirOrdemEntradaRequestValidator : AbstractValidator<DefinirOrdemEntradaRequest>
{
    public DefinirOrdemEntradaRequestValidator()
    {
        _ = RuleFor(x => x.Ordem)
            .GreaterThan(0)
            .WithMessage("A ordem deve ser superior a zero");
    }
}