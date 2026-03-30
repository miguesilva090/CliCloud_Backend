#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Bancos
{
  [Table("Banco", Schema = "Bancos")]
  public class Banco : Entidade 
  {
    // Colunas específicas da tabela [Bancos].[Banco]
    // Descricao: optamos por duplicar o Nome da Entidade para compatibilidade com a BD existente.
    public string? Descricao { get; set; }

    // Abreviatura do banco (ex.: "CGD", "BPI")
    public string? Abreviatura { get; set; }
  }
}