using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs
{
    public class UpdateHistoriaClinicaRequest : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public Guid? EspecialidadeId { get; set; }
        public DateTime Data { get; set; }
        public string? Hora { get; set; }
        public string Obs { get; set; } = string.Empty;
    }

    public class UpdateHistoriaClinicaValidator : AbstractValidator<UpdateHistoriaClinicaRequest>
    {
        public UpdateHistoriaClinicaValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.MedicoId).NotEmpty();
            _ = RuleFor(x => x.EspecialidadeId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
            _ = RuleFor(x => x.Obs).NotEmpty();
        }
    }
}

