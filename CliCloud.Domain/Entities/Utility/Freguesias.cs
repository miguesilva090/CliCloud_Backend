using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Freguesia", Schema = "Utility")]
  public class Freguesia : AuditableEntityWithSoftDelete
  {
    public string Nome {get;set;}
    public Guid ConcelhoId {get;set;}
    public Concelho Concelho {get;set;}
    public ICollection<Rua> Ruas {get;set;} = [];
  }
}