using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs
{
    public class UpdateHorarioMedicoRequest : IDto
    {
        public required string MedicoId { get; set; }
        public int? TipoHorario { get; set; }
        public string? MinMarcacao { get; set; } // TimeSpan como string "HH:mm:ss"
        public bool HoraComp { get; set; }
        public string? PrimeiraConsulta { get; set; } // TimeSpan como string "HH:mm:ss"
        public bool HorarioFlexivel { get; set; }
        // TODO: Adicionar Horarios quando HorarioMedicoDiaService for criado
        // public IEnumerable<UpsertHorarioMedicoDiaRequest>? Horarios { get; set; }
    }

    public class UpdateHorarioMedicoValidator : AbstractValidator<UpdateHorarioMedicoRequest>
    {
        public UpdateHorarioMedicoValidator()
        {
            _ = RuleFor(x => x.MedicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("MedicoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.MinMarcacao)
                .Must(time => string.IsNullOrEmpty(time) || TimeSpan.TryParse(time, out _))
                .WithMessage("MinMarcacao deve ser um TimeSpan válido no formato HH:mm:ss ou vazio.");
            _ = RuleFor(x => x.PrimeiraConsulta)
                .Must(time => string.IsNullOrEmpty(time) || TimeSpan.TryParse(time, out _))
                .WithMessage("PrimeiraConsulta deve ser um TimeSpan válido no formato HH:mm:ss ou vazio.");
        }
    }
}
