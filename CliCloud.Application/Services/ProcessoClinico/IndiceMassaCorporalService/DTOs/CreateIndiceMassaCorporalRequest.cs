using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.IndiceMassaCorporalService.DTOs
{
    public class CreateIndiceMassaCorporalRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateIndiceMassaCorporalValidator : AbstractValidator<CreateIndiceMassaCorporalRequest>
    {
        public CreateIndiceMassaCorporalValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
            _ = RuleFor(x => x.Peso).GreaterThan(0);
            _ = RuleFor(x => x.Altura).GreaterThan(0);
        }
    }
}
