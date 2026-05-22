using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class AppendListaEsperaObservacaoRequest : IDto
{
    public string? Texto { get; set; }
}

public class AppendListaEsperaObservacaoRequestValidator
  : AbstractValidator<AppendListaEsperaObservacaoRequest>
{
    public AppendListaEsperaObservacaoRequestValidator()
    {
        _ = RuleFor(x => x.Texto)
            .NotEmpty()
            .WithMessage("Indique o texto da observação");
    }
}