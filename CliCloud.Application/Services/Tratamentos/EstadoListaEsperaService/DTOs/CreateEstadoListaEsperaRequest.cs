using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs
{
    public class CreateEstadoListaEsperaRequest : IDto
    {
        public string Descricao { get; set; } = string.Empty;
    }

    public class CreateEstadoListaEsperaValidator : AbstractValidator<CreateEstadoListaEsperaRequest>
    {
        public CreateEstadoListaEsperaValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descrição é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
