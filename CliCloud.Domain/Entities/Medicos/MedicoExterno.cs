#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Medicos
{
  [Table("MedicoExterno", Schema = "Medicos")]
  public class MedicoExterno : EntidadePessoa
  {
  }
}
