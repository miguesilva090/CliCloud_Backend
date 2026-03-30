using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sexos.SexoService.DTOs
{
    public class UpdateSexoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateSexoValidator : AbstractValidator<UpdateSexoRequest>
    {
        public UpdateSexoValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}

