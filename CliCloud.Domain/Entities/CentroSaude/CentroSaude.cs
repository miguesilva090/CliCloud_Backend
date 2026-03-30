#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.CentroSaude
{
  [Table("CentroSaude", Schema = "CentroSaude")]
  public class CentroSaude : Entidade
  {
    public string? CodigoLocalCS { get; set; }
  }
}
