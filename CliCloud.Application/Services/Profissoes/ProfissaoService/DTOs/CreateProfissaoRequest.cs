using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs
{
    public class CreateProfissaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateProfissaoValidator : AbstractValidator<CreateProfissaoRequest>
    {
        public CreateProfissaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
