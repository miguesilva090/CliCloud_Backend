using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class AppendListaEsperaTratamentoObservacaoRequest : IDto
{
    public string? Texto { get; set; }
}

public class AppendListaEsperaTratamentoObservacaoRequestValidator
    : AbstractValidator<AppendListaEsperaTratamentoObservacaoRequest>
{
    public AppendListaEsperaTratamentoObservacaoRequestValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty()
            .WithMessage("Indique o texto da observação.");
    }
}
