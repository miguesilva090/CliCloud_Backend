#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Utility
{
  [Table("Entidade", Schema = "Utility")]
  public class Entidade : AuditableEntityWithSoftDelete
  {
    public string Nome {get;set;} = string.Empty;
    public EntidadeTipo TipoEntidade {get;set;}
    public string? Email {get;set;}
    public ICollection<EntidadeContacto> EntidadeContactos {get;set;} = [];
    public string? NumeroContribuinte {get;set;}
    public Guid? RuaId {get;set;}
    public Rua? Rua {get;set;}
    public Guid? CodigoPostalId {get;set;}
    public CodigoPostal? CodigoPostal {get;set;}
    public Guid? FreguesiaId {get;set;}
    public Freguesia? Freguesia {get;set;}
    public Guid? ConcelhoId {get;set;}
    public Concelho? Concelho {get;set;}
    public Guid? DistritoId {get;set;}
    public Distrito? Distrito {get;set;}
    public Guid? PaisId {get;set;}
    public Pais? Pais {get;set;}
    public string? NumeroPorta {get;set;}
    public string? AndarRua {get;set;}
    public string? Observacoes {get;set;}
    public Status? Status {get;set;}
    public string? UrlFoto {get;set;}
  }
}
