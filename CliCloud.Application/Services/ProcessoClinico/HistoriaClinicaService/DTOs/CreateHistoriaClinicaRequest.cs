using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs
{
    public class CreateHistoriaClinicaRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public Guid? EspecialidadeId { get; set; }
        public DateTime Data { get; set; }
        public string? Hora { get; set; }
        public string Obs { get; set; } = string.Empty;
    }

    public class CreateHistoriaClinicaValidator : AbstractValidator<CreateHistoriaClinicaRequest>
    {
        public CreateHistoriaClinicaValidator()
        {
            // Validação simplificada: o front‑end já garante estes campos.
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Obs).NotEmpty();
        }
    }
}
