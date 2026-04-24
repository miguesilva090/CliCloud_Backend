using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Rua", Schema = "Utility")]
  public class Rua : AuditableEntityWithSoftDelete
  {
    public string Nome {get;set;} = string.Empty;
    public Guid FreguesiaId {get;set;}
    public Freguesia Freguesia {get;set;}
    public Guid CodigoPostalId {get;set;}
    public CodigoPostal CodigoPostal {get;set;}
    public ICollection<Entidade> Entidades {get;set;} = [];
  }
}