using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoAntropometricaService.DTOs
{
    public class UpdateAvaliacaoAntropometricaRequest : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal? QuadricepEsq { get; set; }
        public decimal? QuadricepDir { get; set; }
        public decimal? QuadricepDif { get; set; }
        public decimal? IsquiotibialEsq { get; set; }
        public decimal? IsquiotibialDir { get; set; }
        public decimal? IsquiotibialDif { get; set; }
        public decimal? AdutorEsq { get; set; }
        public decimal? AdutorDir { get; set; }
        public decimal? AdutorDif { get; set; }
        public decimal? AbdutorEsq { get; set; }
        public decimal? AbdutorDir { get; set; }
        public decimal? AbdutorDif { get; set; }
        public decimal? GluteoEsq { get; set; }
        public decimal? GluteoDir { get; set; }
        public decimal? GluteoDif { get; set; }
        public decimal? GemeoEsq { get; set; }
        public decimal? GemeoDir { get; set; }
        public decimal? GemeoDif { get; set; }
        public decimal? AbdutorOmbroEsq { get; set; }
        public decimal? AbdutorOmbroDir { get; set; }
        public decimal? AbdutorOmbroDif { get; set; }
        public decimal? FlexorOmbroEsq { get; set; }
        public decimal? FlexorOmbroDir { get; set; }
        public decimal? FlexorOmbroDif { get; set; }
        public decimal? ExtensorOmbroEsq { get; set; }
        public decimal? ExtensorOmbroDir { get; set; }
        public decimal? ExtensorOmbroDif { get; set; }
        public decimal? RotadorInternoOmbroEsq { get; set; }
        public decimal? RotadorInternoOmbroDir { get; set; }
        public decimal? RotadorInternoOmbroDif { get; set; }
        public decimal? RotadorExternoOmbroEsq { get; set; }
        public decimal? RotadorExternoOmbroDir { get; set; }
        public decimal? RotadorExternoOmbroDif { get; set; }
    }

    public class UpdateAvaliacaoAntropometricaValidator : AbstractValidator<UpdateAvaliacaoAntropometricaRequest>
    {
        public UpdateAvaliacaoAntropometricaValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
        }
    }
}
