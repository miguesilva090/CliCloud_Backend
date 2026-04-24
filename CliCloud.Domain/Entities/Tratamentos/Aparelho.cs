#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("Aparelho", Schema = "Tratamentos")]
  public class Aparelho : AuditableEntityWithSoftDelete
  {
    [Key]
    public new Guid Id { get; set; }

    public Guid TipoAparelhoId { get; set; }
    public TipoAparelho TipoAparelho { get; set; } = null!;

    public Guid? ModeloAparelhoId { get; set; }
    public ModeloAparelho? ModeloAparelho { get; set; }

    [StringLength(50)]
    public string? CodigoSerie { get; set; }

    [StringLength(50)]
    public string? CodigoInventario { get; set; }

    [StringLength(100)]
    public string? Local { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }

    public bool Ocupado { get; set; }
  }
}
