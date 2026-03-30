using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.AlergiaUtenteService.DTOs
{
    public class UpdateAlergiaUtenteRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid? AlergiaId { get; set; }
        public Guid? GrauAlergiaId { get; set; }
        public DateOnly? DataDesde { get; set; }
        public DateOnly? DataAte { get; set; }
        public string? Observacoes { get; set; }
    }

    public class UpdateAlergiaUtenteValidator : AbstractValidator<UpdateAlergiaUtenteRequest>
    {
        public UpdateAlergiaUtenteValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
        }
    }
}
