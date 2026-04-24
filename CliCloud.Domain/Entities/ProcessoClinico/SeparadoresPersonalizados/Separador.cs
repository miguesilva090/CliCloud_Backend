#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

[Table("Separador", Schema = "ProcessoClinico")]
public class Separador : AuditableEntityWithSoftDelete
{
    [Key]
    public new Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    public int Ordem { get; set; }

    public bool Ativo { get; set; } = true;
}

