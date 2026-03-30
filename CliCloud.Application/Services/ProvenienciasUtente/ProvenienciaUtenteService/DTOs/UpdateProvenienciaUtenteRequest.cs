using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs
{
    public class UpdateProvenienciaUtenteRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateProvenienciaUtenteValidator : AbstractValidator<UpdateProvenienciaUtenteRequest>
    {
        public UpdateProvenienciaUtenteValidator()
        {
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(80)
                .WithMessage("Descricao é obrigatória e deve ter no máximo 80 caracteres.");
        }
    }
}
