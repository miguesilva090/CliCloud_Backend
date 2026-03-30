using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs
{
    public class UpdateAntecedentesCirurgicosRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public int? Ano { get; set; }
        public string? TipoCirurgia { get; set; }
        public bool? HouveComplicacoes { get; set; }
        public string? Complicacoes { get; set; }
        public string? Observacoes { get; set; }
    }

    public class UpdateAntecedentesCirurgicosValidator : AbstractValidator<UpdateAntecedentesCirurgicosRequest>
    {
        public UpdateAntecedentesCirurgicosValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Ano).NotEmpty();
            _ = RuleFor(x => x.TipoCirurgia).NotEmpty();
        }
    }
}

