using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs
{
    public class UpdateHorarioTecnicoRequest : IDto
    {
        public required string TecnicoId { get; set; }
        public int? TipoHorario { get; set; }
        public string? MinMarcacao { get; set; }
        public int? HoraComp { get; set; }
        // TODO: Adicionar Horarios quando HorarioTecnicoDiaService for criado
        // public IEnumerable<UpsertHorarioTecnicoDiaRequest>? Horarios { get; set; }
    }

    public class UpdateHorarioTecnicoValidator : AbstractValidator<UpdateHorarioTecnicoRequest>
    {
        public UpdateHorarioTecnicoValidator()
        {
            _ = RuleFor(x => x.TecnicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("TecnicoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.MinMarcacao)
                .MaximumLength(50)
                .WithMessage("MinMarcacao deve ter no máximo 50 caracteres.");
            _ = RuleFor(x => x.HoraComp)
                .GreaterThanOrEqualTo(0)
                .When(x => x.HoraComp.HasValue)
                .WithMessage("HoraComp deve ser um número positivo ou zero.");
        }
    }
}
