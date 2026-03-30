using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs
{
    public class CreateGrauParentescoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateGrauParentescoValidator : AbstractValidator<CreateGrauParentescoRequest>
    {
        public CreateGrauParentescoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
