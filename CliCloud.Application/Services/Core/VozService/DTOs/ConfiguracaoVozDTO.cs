using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.VozService.DTOs
{
  public class ConfiguracaoVozDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public bool Ativo { get; set; }

    public string Provider { get; set; } = "web-speech";
    public string IdiomaPadrao { get; set; } = "pt-PT";
    public string SttIdioma { get; set; } = "pt-PT";

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
}
