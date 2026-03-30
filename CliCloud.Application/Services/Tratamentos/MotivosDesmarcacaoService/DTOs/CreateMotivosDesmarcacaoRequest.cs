using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs
{
    public class CreateMotivosDesmarcacaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateMotivosDesmarcacaoValidator : AbstractValidator<CreateMotivosDesmarcacaoRequest>
    {
        public CreateMotivosDesmarcacaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 100 caracteres.");
        }
    }
}
