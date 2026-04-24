using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ClinicaConfiguracaoIva", Schema = "Core")]
  public class ClinicaConfiguracaoIva : AuditableEntityWithSoftDelete
  {
    public new Guid Id { get; set; }
    public Guid ClinicaId { get; set; }
    public int Ano { get; set; }

    public Guid? TaxaIva0Id { get; set; }
    public Guid? TaxaIva1Id { get; set; }
    public Guid? TaxaIva2Id { get; set; }
    public Guid? TaxaIva3Id { get; set; }
    public Guid? TaxaIva4Id { get; set; }
    public Guid? TaxaIva5Id { get; set; }
    public Guid? TaxaIva6Id { get; set; }
    public Guid? TaxaIva7Id { get; set; }
    public Guid? TaxaIva8Id { get; set; }
    public Guid? TaxaIva9Id { get; set; }
  }
}
