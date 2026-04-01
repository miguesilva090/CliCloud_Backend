#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ConfiguracaoChamadaVozOpcao", Schema = "Core")]
  public class ConfiguracaoChamadaVozOpcao : AuditableEntity
  {
    [StringLength(20)]
    public string Tipo { get; set; } = null!; // Language | Tld

    [StringLength(30)]
    public string Codigo { get; set; } = null!;

    [StringLength(120)]
    public string Descricao { get; set; } = null!;

    public int Ordem { get; set; }

    public bool Ativo { get; set; } = true;
  }
}
