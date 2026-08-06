#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Servicos
{
  [Table("TipoServico", Schema = "Servicos")]
  public class TipoServico : AuditableEntityWithSoftDelete
  {
    [Required]
    [StringLength(80)]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Código de negócio usado nos lotes (paridade c_tipo_srv).</summary>
    public int? Codigo { get; set; }

    /// <summary>Âmbito clínica (paridade filtro; alinhar com Clinica.Cid).</summary>
    public int? Filtro { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TaxaModeradoraSns { get; set; }

    public bool PartilhaSemRequisicao { get; set; }

    public ICollection<Servico> Servicos { get; set; } = [];
  }
}
