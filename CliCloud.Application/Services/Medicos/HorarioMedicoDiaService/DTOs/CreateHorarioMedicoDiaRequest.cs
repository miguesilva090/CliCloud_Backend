using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs
{
    public class CreateHorarioMedicoDiaRequest : IDto
    {
        public required string HorarioMedicoId { get; set; }
        public required DiaSemana DiaSemana { get; set; }
        public required Periodo Periodo { get; set; }
        public string? Inicio { get; set; } // TimeSpan como string "HH:mm:ss"
        public string? Fim { get; set; } // TimeSpan como string "HH:mm:ss"
        public string? Sala { get; set; }
        public int? Vagas { get; set; }
    }

    public class CreateHorarioMedicoDiaValidator : AbstractValidator<CreateHorarioMedicoDiaRequest>
    {
        public CreateHorarioMedicoDiaValidator()
        {
            _ = RuleFor(x => x.HorarioMedicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("HorarioMedicoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DiaSemana)
                .IsInEnum()
                .WithMessage("DiaSemana deve ser um valor válido do enum.");
            _ = RuleFor(x => x.Periodo)
                .IsInEnum()
                .WithMessage("Periodo deve ser um valor válido do enum.");
            _ = RuleFor(x => x.Inicio)
                .Must(time => string.IsNullOrEmpty(time) || TimeSpan.TryParse(time, out _))
                .WithMessage("Inicio deve ser um TimeSpan válido no formato HH:mm:ss ou vazio.");
            _ = RuleFor(x => x.Fim)
                .Must(time => string.IsNullOrEmpty(time) || TimeSpan.TryParse(time, out _))
                .WithMessage("Fim deve ser um TimeSpan válido no formato HH:mm:ss ou vazio.");
            _ = RuleFor(x => x.Sala)
                .MaximumLength(50)
                .WithMessage("Sala deve ter no máximo 50 caracteres.");
            _ = RuleFor(x => x.Vagas)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Vagas.HasValue)
                .WithMessage("Vagas deve ser um número positivo ou zero.");
        }
    }
}
