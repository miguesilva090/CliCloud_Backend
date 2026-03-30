using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.DTOs
{
    public class CreateMotivoAltaRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateMotivoAltaValidator : AbstractValidator<CreateMotivoAltaRequest>
    {
        public CreateMotivoAltaValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
