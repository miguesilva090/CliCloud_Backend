using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.DTOs
{
    public class CreateTipoDeDorRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateTipoDeDorValidator : AbstractValidator<CreateTipoDeDorRequest>
    {
        public CreateTipoDeDorValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
