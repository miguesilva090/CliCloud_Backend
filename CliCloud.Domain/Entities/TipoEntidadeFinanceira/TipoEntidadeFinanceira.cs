#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.TipoEntidadeFinanceira
{
  [Table("TipoEntidadeFinanceira", Schema = "TipoEntidadeFinanceira")]
  public class TipoEntidadeFinanceira : AuditableEntity
  {
    [Required]
    [StringLength(20)]
    public string Sigla { get; set; } = string.Empty;
    
    [Required]
    [StringLength(254)]
    public string Designacao { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Dominio { get; set; } = string.Empty;
    
    [Required]
    [StringLength(254)]
    public string DescricaoDominio { get; set; } = string.Empty;
  }
}