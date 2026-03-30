using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("CodigoPostal", Schema = "Utility")]
  public class CodigoPostal : AuditableEntity
  {
    public string Codigo {get;set;} = string.Empty;
    public string Localidade {get;set;} = string.Empty;
    public ICollection<Rua> Ruas {get;set;} = [];
  }
}