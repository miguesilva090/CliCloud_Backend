using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs
{
    public class UpdateEstadoCivilRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateEstadoCivilValidator : AbstractValidator<UpdateEstadoCivilRequest>
    {
        public UpdateEstadoCivilValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
