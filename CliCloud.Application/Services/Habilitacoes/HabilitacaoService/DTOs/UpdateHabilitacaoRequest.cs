using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Habilitacoes.HabilitacaoService.DTOs
{
    public class UpdateHabilitacaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateHabilitacaoValidator : AbstractValidator<UpdateHabilitacaoRequest>
    {
        public UpdateHabilitacaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
