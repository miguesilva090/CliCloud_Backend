#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;
using TipoEntidadeFinanceiraEntity = CliCloud.Domain.Entities.TipoEntidadeFinanceira.TipoEntidadeFinanceira;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.EntidadesFinanceiras
{
  [Table("EntidadeFinanceira", Schema = "EntidadesFinanceiras")]
  public class EntidadeFinanceira : Entidade
  {
    [StringLength(50)]
    public string? Abreviatura { get; set; }

    [Required]
    [StringLength(20)]
    public string PaisPrefixo { get; set; } = string.Empty;

    [Required]
    public Guid TipoEntidadeFinanceiraId { get; set; }
    public TipoEntidadeFinanceiraEntity? TipoEntidadeFinanceira { get; set; }

    public CondicaoSns? CondicaoSns { get; set; }
  }
}
