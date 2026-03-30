using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.GorduraMassaMuscularService.DTOs
{
    public class CreateGorduraMassaMuscularRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal? PercentAguaCorpo { get; set; }
        public decimal? PercentGordPernaDir { get; set; }
        public decimal? PercentGordPernaEsq { get; set; }
        public decimal? PercentGordBracoDir { get; set; }
        public decimal? PercentGordBracoEsq { get; set; }
        public decimal? PercentGordTronco { get; set; }
        public decimal? GorduraVisceral { get; set; }
        public decimal? MassaMuscPernaDir { get; set; }
        public decimal? MassaMuscPernaEsq { get; set; }
        public decimal? MassaMuscBracoDir { get; set; }
        public decimal? MassaMuscBracoEsq { get; set; }
        public decimal? MassaMuscTronco { get; set; }
        public decimal? ConsumoMetabolico { get; set; }
    }

    public class CreateGorduraMassaMuscularValidator : AbstractValidator<CreateGorduraMassaMuscularRequest>
    {
        public CreateGorduraMassaMuscularValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
        }
    }
}
