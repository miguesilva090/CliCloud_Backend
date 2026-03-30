using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs
{
    public class UpdateProfissaoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateProfissaoValidator : AbstractValidator<UpdateProfissaoRequest>
    {
        public UpdateProfissaoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
