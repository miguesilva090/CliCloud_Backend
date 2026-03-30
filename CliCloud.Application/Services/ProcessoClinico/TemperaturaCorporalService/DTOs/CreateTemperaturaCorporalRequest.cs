using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TemperaturaCorporalService.DTOs
{
    public class CreateTemperaturaCorporalRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal Temperatura { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateTemperaturaCorporalValidator : AbstractValidator<CreateTemperaturaCorporalRequest>
    {
        public CreateTemperaturaCorporalValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
            _ = RuleFor(x => x.Temperatura).GreaterThan(0);
        }
    }
}
