using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Antecedentes.AntecedentesFamiliaresUtenteService.DTOs
{
    public class UpdateAntecedentesFamiliaresUtenteRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid? DoencaId { get; set; }
        public int? Ano { get; set; }
        public int? Idade { get; set; }
        public DateTime? Data { get; set; }
        public Guid GrauParentescoId { get; set; }
    }

    public class UpdateAntecedentesFamiliaresUtenteValidator : AbstractValidator<UpdateAntecedentesFamiliaresUtenteRequest>
    {
        public UpdateAntecedentesFamiliaresUtenteValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.DoencaId).NotEmpty();
            _ = RuleFor(x => x.Ano).GreaterThan(0);
            _ = RuleFor(x => x.Idade).GreaterThan(0);
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.GrauParentescoId).NotEmpty();
        }
    }
}
