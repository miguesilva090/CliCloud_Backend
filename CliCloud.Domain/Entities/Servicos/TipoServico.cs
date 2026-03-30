#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Servicos
{
  [Table("TipoServico", Schema = "Servicos")]
  public class TipoServico : AuditableEntity
  {
    [Required]
    [StringLength(80)]
    public string Descricao { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxaModeradoraSns { get; set; }
  
    public bool PartilhaSemRequisicao { get; set; }

    public ICollection<Servico> Servicos { get; set; } = [];
  }
}
