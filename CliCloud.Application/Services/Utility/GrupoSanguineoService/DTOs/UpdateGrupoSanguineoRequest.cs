using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs
{
    public class UpdateGrupoSanguineoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateGrupoSanguineoValidator : AbstractValidator<UpdateGrupoSanguineoRequest>
    {
        public UpdateGrupoSanguineoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descrição é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
