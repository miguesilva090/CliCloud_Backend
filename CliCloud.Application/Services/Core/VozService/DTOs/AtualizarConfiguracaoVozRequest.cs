using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Core.VozService.DTOs
{
  public class AtualizarConfiguracaoVozRequest : IDto
  {
    public bool Ativo { get; set; }

    public string? Provider { get; set; }
    public string? IdiomaPadrao { get; set; }
    public string? SttIdioma { get; set; }

    public bool SttAtivo { get; set; }
    public bool SttInterimResults { get; set; }
    public bool SttContinuous { get; set; }
    public bool SttAutoPontuacao { get; set; }

    public decimal SttConfidenceMin { get; set; }
    public int SttSilenceTimeoutMs { get; set; }
    public int SttMaxAlternatives { get; set; }
    public bool SttProfanityFilter { get; set; }

    public bool TtsAtivo { get; set; }
    public string? TtsVoice { get; set; }
    public decimal TtsRate { get; set; }
    public decimal TtsPitch { get; set; }
    public decimal TtsVolume { get; set; }

    public int TimeoutMs { get; set; }
    public int MaxDuracaoCapturaSegundos { get; set; }
  }

  public class AtualizarConfiguracaoVozValidator : AbstractValidator<AtualizarConfiguracaoVozRequest>
  {
    public AtualizarConfiguracaoVozValidator()
    {
      RuleFor(x => x.Provider).MaximumLength(50);
      RuleFor(x => x.IdiomaPadrao).MaximumLength(10);
      RuleFor(x => x.SttIdioma).MaximumLength(10);
      RuleFor(x => x.TtsVoice).MaximumLength(100);

      RuleFor(x => x.SttConfidenceMin).InclusiveBetween(0m, 1m);
      RuleFor(x => x.SttSilenceTimeoutMs).InclusiveBetween(500, 10000);
      RuleFor(x => x.SttMaxAlternatives).InclusiveBetween(1, 5);
      RuleFor(x => x.TtsRate).InclusiveBetween(0.5m, 2m);
      RuleFor(x => x.TtsPitch).InclusiveBetween(0m, 2m);
      RuleFor(x => x.TtsVolume).InclusiveBetween(0m, 1m);

      RuleFor(x => x.TimeoutMs).InclusiveBetween(1000, 60000);
      RuleFor(x => x.MaxDuracaoCapturaSegundos).InclusiveBetween(10, 600);

      RuleFor(x => x.Provider)
        .NotEmpty()
        .When(x => x.Ativo)
        .WithMessage("Provider é obrigatório quando o serviço está ativo.");

      RuleFor(x => x.IdiomaPadrao)
        .NotEmpty()
        .When(x => x.Ativo)
        .WithMessage("Idioma padrão é obrigatório quando o serviço está ativo.");

      RuleFor(x => x.SttIdioma)
        .NotEmpty()
        .When(x => x.SttAtivo)
        .WithMessage("Idioma do STT é obrigatório quando STT está ativo.");
    }
  }
}
