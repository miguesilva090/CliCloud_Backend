#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ConfiguracaoVoz", Schema = "Core")]
  public class ConfiguracaoVoz : AuditableEntityWithSoftDelete
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;
    public bool Ativo { get; set; }

    [StringLength(50)]
    public string Provider { get; set; } = "web-speech";

    [StringLength(10)]
    public string IdiomaPadrao { get; set; } = "pt-PT";

    [StringLength(10)]
    public string SttIdioma { get; set; } = "pt-PT";

    public bool SttAtivo { get; set; }
    public bool SttInterimResults { get; set; }
    public bool SttContinuous { get; set; }
    public bool SttAutoPontuacao { get; set; }
    public decimal SttConfidenceMin { get; set; } = 0.5m;
    public int SttSilenceTimeoutMs { get; set; } = 2500;
    public int SttMaxAlternatives { get; set; } = 1;
    public bool SttProfanityFilter { get; set; }
    public bool TtsAtivo { get; set; }

    [StringLength(100)]
    public string? TtsVoice { get; set; }

    public decimal TtsRate { get; set; } = 1.0m;
    public decimal TtsPitch { get; set; } = 1.0m;
    public decimal TtsVolume { get; set; } = 1.0m;
    public int TimeoutMs { get; set; } = 15000;
    public int MaxDuracaoCapturaSegundos { get; set; } = 90;
  }
}
