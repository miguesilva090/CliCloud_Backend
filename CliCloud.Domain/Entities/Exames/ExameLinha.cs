#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Exames
{
  /// <summary>
  /// Linha da prescrição de exames: um tipo de exame prescrito com quantidade (e opcionalmente recomendações).
  /// Cód. e Designação vêm do TipoExame associado.
  /// </summary>
  [Table("ExameLinha", Schema = "Exames")]
  public class ExameLinha : AuditableEntity
  {
    [Required]
    public Guid ExameId { get; set; }
    [ForeignKey("ExameId")]
    public Exame Exame { get; set; } = null!;

    [Required]
    public Guid TipoExameId { get; set; }
    [ForeignKey("TipoExameId")]
    public TipoExame TipoExame { get; set; } = null!;

    public int Quantidade { get; set; } = 1;

    [StringLength(500)]
    public string? Recomendacoes { get; set; }

    /// <summary>
    /// Valor/resultados do exame (campo livre), preenchido no ecrã "Resultados de Exames".
    /// </summary>
    [StringLength(1000)]
    public string? ResultadoValor { get; set; }

    /// <summary>
    /// Valores de referência apresentados no ecrã "Resultados de Exames".
    /// </summary>
    [StringLength(500)]
    public string? ResultadoReferencia { get; set; }

    /// <summary>
    /// Observações adicionais / texto completo do resultado, caso necessário.
    /// </summary>
    [StringLength(2000)]
    public string? ResultadoObs { get; set; }
  }
}
