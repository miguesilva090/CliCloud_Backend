using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.AvaliacaoPosturalService.DTOs
{
    public class UpdateAvaliacaoPosturalRequest : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }

        // Olho
        public string? EndoforiaPresente { get; set; }
        public string? EndoforiaValores { get; set; }
        public string? ExoforiaPresente { get; set; }
        public string? ExoforiaValores { get; set; }
        public string? HiperforiaPresente { get; set; }
        public string? HiperforiaValores { get; set; }
        public string? DesvioPresente { get; set; }
        public string? DesvioValores { get; set; }

        // Captores - ATM
        public string? AberturaPresente { get; set; }
        public string? AberturaValores { get; set; }
        public string? FechoPresente { get; set; }
        public string? FechoValores { get; set; }

        // Pé
        public string? SupinadoPresente { get; set; }
        public string? SupinadoValores { get; set; }
        public string? PronadoPresente { get; set; }
        public string? PronadoValores { get; set; }
        public string? NeutroPresente { get; set; }
        public string? NeutroValores { get; set; }

        // Coluna
        public string? EscoliosePresente { get; set; }
        public string? EscolioseValores { get; set; }
        public string? HipercifosePresente { get; set; }
        public string? HipercifoseValores { get; set; }
        public string? HiperlordosePresente { get; set; }
        public string? HiperlordoseValores { get; set; }

        // Perna
        public string? CurtaPresente { get; set; }
        public string? CurtaValores { get; set; }
        public string? ValgoPresente { get; set; }
        public string? ValgoValores { get; set; }
        public string? VaroPresente { get; set; }
        public string? VaroValores { get; set; }

        // Anca/Bacia
        public string? IliacoPresente { get; set; }
        public string? IliacoValores { get; set; }
        public string? SacroPresente { get; set; }
        public string? SacroValores { get; set; }
        public string? SubidoPresente { get; set; }
        public string? SubidoValores { get; set; }

        // Ombro
        public string? DescidoPresente { get; set; }
        public string? DescidoValores { get; set; }
        public string? AnteriorPresente { get; set; }
        public string? AnteriorValores { get; set; }
        public string? PosteriorPresente { get; set; }
        public string? PosteriorValores { get; set; }
        public string? RotacaoPresente { get; set; }
        public string? RotacaoValores { get; set; }

        // Cabeça
        public string? InclinacaoPresente { get; set; }
        public string? InclinacaoValores { get; set; }

        // Outros
        public string? Outros1Presente { get; set; }
        public string? Outros1Valores { get; set; }
        public string? Outros2Presente { get; set; }
        public string? Outros2Valores { get; set; }
        public string? Outros3Presente { get; set; }
        public string? Outros3Valores { get; set; }
    }

    public class UpdateAvaliacaoPosturalValidator : AbstractValidator<UpdateAvaliacaoPosturalRequest>
    {
        public UpdateAvaliacaoPosturalValidator()
        {
            _ = RuleFor(x => x.Id).NotEmpty();
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Data).NotEmpty();
            _ = RuleFor(x => x.Hora).NotEmpty();
        }
    }
}
