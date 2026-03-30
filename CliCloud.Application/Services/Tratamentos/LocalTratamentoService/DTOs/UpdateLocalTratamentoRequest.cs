using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.DTOs
{
    public class UpdateLocalTratamentoRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class UpdateLocalTratamentoValidator : AbstractValidator<UpdateLocalTratamentoRequest>
    {
        public UpdateLocalTratamentoValidator()
        {
            _ = RuleFor(x => x.Designacao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Designação é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
