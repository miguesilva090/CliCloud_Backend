#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Domain.Entities.Tratamentos
{
  /// <summary>
  /// Patologia (TRATPRED no legado). Designação, organismo, local de tratamento, especificação técnica.
  /// Doenças: no legado é campo texto (Doencas); opcionalmente associadas via PatologiaDoencas (N-N).
  /// </summary>
  [Table("Patologias", Schema = "Tratamentos")]
  public class Patologia : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Designacao { get; set; } = string.Empty;

    /// <summary>FK local de tratamento (c_loctrat no legado).</summary>
    public Guid? LocalTratamentoId { get; set; }
    public LocalTratamento? LocalTratamento { get; set; }

    /// <summary>FK organismo/instituição (c_instit no legado).</summary>
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? EspecificacaoTecnica { get; set; }

    /// <summary>Doenças em texto (campo doencas no legado TRATPRED).</summary>
    [Column(TypeName = "nvarchar(max)")]
    public string? Doencas { get; set; }

    public bool Inativo { get; set; }

    public ICollection<PatologiaServico> PatologiaServicos { get; set; } = new List<PatologiaServico>();
    public ICollection<PatologiaDoenca> PatologiaDoencas { get; set; } = new List<PatologiaDoenca>();
  }
}
