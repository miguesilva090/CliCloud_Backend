using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sexos.SexoService.DTOs
{
    public class CreateSexoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateSexoValidator : AbstractValidator<CreateSexoRequest>
    {
        public CreateSexoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}

