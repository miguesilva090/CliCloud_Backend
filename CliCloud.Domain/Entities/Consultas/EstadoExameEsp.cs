#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Consultas;

[Table("EstadoExameEsp", Schema = "Consultas")]
public class EstadoExameEsp : AuditableEntity
{
    public int Codigo { get; set; }

    [StringLength(4)]
    public string Abreviatura { get; set; } = string.Empty;

    [StringLength(20)]
    public string Descricao { get; set; } = string.Empty;
}
