using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Concelho", Schema = "Utility")]
  public class Concelho : AuditableEntityWithSoftDelete
  {
    public string Nome {get;set;}
    public Guid DistritoId {get;set;}
    public Distrito Distrito {get;set;}
    public ICollection<Freguesia> Freguesias {get;set;} = [];
  }
}