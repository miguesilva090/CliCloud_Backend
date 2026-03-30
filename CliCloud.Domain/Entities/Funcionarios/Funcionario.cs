#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Funcionarios
{
  [Table("Funcionario", Schema = "Funcionarios")]
  public class Funcionario : EntidadePessoa
  {
    
  }
}