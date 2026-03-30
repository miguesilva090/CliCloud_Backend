using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.DTOs
{
    public class UpdateEstadoListaEsperaRequest : IDto
    {
        public string Descricao { get; set; } = string.Empty;
    }

    public class UpdateEstadoListaEsperaValidator : AbstractValidator<UpdateEstadoListaEsperaRequest>
    {
        public UpdateEstadoListaEsperaValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descrição é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
