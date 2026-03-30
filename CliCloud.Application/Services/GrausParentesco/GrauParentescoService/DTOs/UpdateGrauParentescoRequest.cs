using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs
{
    public class UpdateGrauParentescoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateGrauParentescoValidator : AbstractValidator<UpdateGrauParentescoRequest>
    {
        public UpdateGrauParentescoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
