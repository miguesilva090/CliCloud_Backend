using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.DTOs
{
    public class CreateGoniometriasRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateGoniometriasValidator : AbstractValidator<CreateGoniometriasRequest>
    {
        public CreateGoniometriasValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
