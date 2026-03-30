using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.DTOs
{
    public class CreateModeloAparelhoRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
        public Guid MarcaAparelhoId { get; set; }
    }

    public class CreateModeloAparelhoValidator : AbstractValidator<CreateModeloAparelhoRequest>
    {
        public CreateModeloAparelhoValidator()
        {
            _ = RuleFor(x => x.Designacao)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
            _ = RuleFor(x => x.MarcaAparelhoId)
            .NotEmpty()
            .WithMessage("MarcaAparelhoId é obrigatório.");
        }
    }
}
