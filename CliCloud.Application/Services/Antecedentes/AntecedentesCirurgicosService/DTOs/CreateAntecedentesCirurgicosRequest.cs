using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs
{
    public class CreateAntecedentesCirurgicosRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public int? Ano { get; set; }
        public string? TipoCirurgia { get; set; }
        public bool? HouveComplicacoes { get; set; }
        public string? Complicacoes { get; set; }
        public string? Observacoes { get; set; }
    }

    public class CreateAntecedentesCirurgicosValidator : AbstractValidator<CreateAntecedentesCirurgicosRequest>
    {
        public CreateAntecedentesCirurgicosValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Ano).NotEmpty();
            _ = RuleFor(x => x.TipoCirurgia).NotEmpty();
            
        }
    }
}
