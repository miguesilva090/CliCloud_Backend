using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs
{
    public class CreateFraquezasMuscularesRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateFraquezasMuscularesValidator : AbstractValidator<CreateFraquezasMuscularesRequest>
    {
        public CreateFraquezasMuscularesValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
