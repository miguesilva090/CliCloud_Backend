#nullable enable
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("EntidadeContacto", Schema = "Utility")]
  public class EntidadeContacto : AuditableEntity
  {
    public int EntidadeContactoTipoId {get;set;}
    public Guid EntidadeId {get;set;}
    public Entidade? Entidade {get;set;}
    public string? Indicativo {get;set;}
    public string? Valor {get;set;} 
    public bool Principal {get;set;}
  }
}