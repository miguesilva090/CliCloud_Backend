using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Pais", Schema = "Utility")]
  public class Pais : AuditableEntity
  {
    public string Codigo {get;set;}
    public string Nome {get;set;}
    public string Prefixo {get;set;}
    public ICollection<Distrito> Distritos {get;set;} = [];
  }
}