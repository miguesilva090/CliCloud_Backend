#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.ProcessoClinico
{
  [Table("QuestionariosUtente", Schema = "ProcessoClinico")]
  public class QuestionarioUtente : AuditableEntity
  {
    public Guid UtenteId { get; set; }
    public Utente Utente { get; set; } = null!;

    public DateTime DataCriacao { get; set; }

    // Tratamento médico atual
    public bool EstaFazerTratamento { get; set; }

    [StringLength(200)]
    public string? DuracaoTratamento { get; set; }

    [StringLength(500)]
    public string? DescTratamento { get; set; }

    // Problemas de saúde (checkboxes principais)
    public bool ProblemasNeurologicos { get; set; }
    public bool ProblemasNeuromusculares { get; set; }
    public bool ProblemasRespiratorios { get; set; }
    public bool ProblemasArticularesOuReumatismo { get; set; }
    public bool TonturasEZumbidosNosOuvidos { get; set; }
    public bool ProblemasComportamento { get; set; }
    public bool ProblemasRenais { get; set; }
    public bool TonturasCardiovasculares { get; set; }
    public bool ProblemasDigestivos { get; set; }
    public bool ProblemasGastricos { get; set; }
    public bool FebreReumatica { get; set; }
    public bool ProblemasIntestinais { get; set; }
    public bool ProblemasUrinarios { get; set; }
    public bool ProblemasComAnestesia { get; set; }
    public bool ProblemasDeHomorragia { get; set; }
    public bool ProblemasFormigueiroDormencia { get; set; }
    public bool ProblemasCicratizacao { get; set; }
    public bool HepatiteSida { get; set; }
    public bool Gravidez { get; set; }
    public bool SedeEBocaSeca { get; set; }
    public bool UsaPacemaker { get; set; }

    public bool? DoencaAutoImune { get; set; }
    public bool? DoencaCronica { get; set; }
    public bool? DoencaGenetica { get; set; }

    // Campos com subtipos (radio buttons)
    public bool ProblemasDiabetes { get; set; }
    /// <summary>
    /// 0 = Tipo 1, 1 = Tipo 2, 2 = Gestacional
    /// </summary>
    public int? TipoDiabetes { get; set; }

    public bool TensaoArterial { get; set; }
    /// <summary>
    /// 0 = Alta, 1 = Baixa, 2 = Normal
    /// </summary>
    public int? TipoTensaoArterial { get; set; }

    public bool Colestrol { get; set; }
    /// <summary>
    /// 0 = Desejável, 1 = Limítrofe, 2 = Alto
    /// </summary>
    public int? TipoColestrol { get; set; }

    [StringLength(2000)]
    public string? Observacoes { get; set; }
  }
}

