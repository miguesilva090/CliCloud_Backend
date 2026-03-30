#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Tratamentos
{
  /// <summary>
  /// Linha Organismo/Serviço da patologia (STRATPRE no legado).
  /// Valores (utente, instituição, preço) são guardados na linha como no legado.
  /// </summary>
  [Table("PatologiaServico", Schema = "Tratamentos")]
  public class PatologiaServico : AuditableEntity
  {
    public Guid PatologiaId { get; set; }
    public Patologia Patologia { get; set; } = null!;

    public Guid SubsistemaServicoId { get; set; }
    public SubsistemaServico SubsistemaServico { get; set; } = null!;

    [StringLength(20)]
    public string? Duracao { get; set; }

    public int Ordem { get; set; }

    public bool Fisioterapia { get; set; }
    public bool Auxiliar { get; set; }

    /// <summary>Valor utente (valor_ut no legado).</summary>
    public decimal ValorUtente { get; set; }

    /// <summary>Valor instituição/organismo (valor_desc no legado).</summary>
    public decimal ValorOrganismo { get; set; }

    /// <summary>Percentagem instituição (desc_inst no legado).</summary>
    public decimal? PercentagemInstituicao { get; set; }

    /// <summary>Preço total em EUR (preco no legado).</summary>
    public decimal? PrecoEur { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }
  }
}
