using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Distrito", Schema = "Utility")]
  public class Distrito : AuditableEntityWithSoftDelete
  {
    public string Nome {get;set;}
    public Guid PaisId {get;set;}
    public Pais Pais {get;set;}
    public ICollection<Concelho> Concelhos {get;set;} = [];
  }
}