using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs
{
    public class CreateEstadoCivilRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateEstadoCivilValidator : AbstractValidator<CreateEstadoCivilRequest>
    {
        public CreateEstadoCivilValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
