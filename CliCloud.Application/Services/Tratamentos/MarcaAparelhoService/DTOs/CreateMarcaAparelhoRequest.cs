using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.DTOs
{
    public class CreateMarcaAparelhoRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class CreateMarcaAparelhoValidator : AbstractValidator<CreateMarcaAparelhoRequest>
    {
        public CreateMarcaAparelhoValidator()
        {
            _ = RuleFor(x => x.Designacao)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
