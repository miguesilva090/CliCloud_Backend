using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs
{
    public class CreateLocalTratamentoRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class CreateLocalTratamentoValidator : AbstractValidator<CreateLocalTratamentoRequest>
    {
        public CreateLocalTratamentoValidator()
        {
            _ = RuleFor(x => x.Designacao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Designação é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
