using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs
{
    public class CreateProvenienciaUtenteRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateProvenienciaUtenteValidator : AbstractValidator<CreateProvenienciaUtenteRequest>
    {
        public CreateProvenienciaUtenteValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
