using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GlicemiaCapilarService.DTOs
{
    public class CreateGlicemiaCapilarRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public int Glicemia { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateGlicemiaCapilarValidator : AbstractValidator<CreateGlicemiaCapilarRequest>
    {
        public CreateGlicemiaCapilarValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
            _ = RuleFor(x => x.Glicemia).GreaterThanOrEqualTo(0);
        }
    }
}
