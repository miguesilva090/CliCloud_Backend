using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs
{
    public class UpdateHorarioTecnicoDiaRequest : IDto
    {
        public required string HorarioTecnicoId { get; set; }
        public required DiaSemana DiaSemana { get; set; }
        public required Periodo Periodo { get; set; }
        public string? Inicio { get; set; }
        public string? Fim { get; set; }
        public string? Sala { get; set; }
        public int? NumMarcacoesPeriodo { get; set; }
        public int? NumMarcacoesOutro { get; set; }
    }

    public class UpdateHorarioTecnicoDiaValidator : AbstractValidator<UpdateHorarioTecnicoDiaRequest>
    {
        public UpdateHorarioTecnicoDiaValidator()
        {
            _ = RuleFor(x => x.HorarioTecnicoId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("HorarioTecnicoId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.DiaSemana)
                .IsInEnum()
                .WithMessage("DiaSemana deve ser um valor válido do enum.");
            _ = RuleFor(x => x.Periodo)
                .IsInEnum()
                .WithMessage("Periodo deve ser um valor válido do enum.");
            _ = RuleFor(x => x.Sala)
                .MaximumLength(50)
                .WithMessage("Sala deve ter no máximo 50 caracteres.");
            _ = RuleFor(x => x.NumMarcacoesPeriodo)
                .GreaterThanOrEqualTo(0)
                .When(x => x.NumMarcacoesPeriodo.HasValue)
                .WithMessage("NumMarcacoesPeriodo deve ser um número positivo ou zero.");
            _ = RuleFor(x => x.NumMarcacoesOutro)
                .GreaterThanOrEqualTo(0)
                .When(x => x.NumMarcacoesOutro.HasValue)
                .WithMessage("NumMarcacoesOutro deve ser um número positivo ou zero.");
        }
    }
}
