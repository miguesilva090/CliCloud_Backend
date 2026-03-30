using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs
{
    public class CreateHabilitacaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateHabilitacaoValidator : AbstractValidator<CreateHabilitacaoRequest>
    {
        public CreateHabilitacaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
