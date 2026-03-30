#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Medicos
{
  [Table("HorarioMedicoVariavel", Schema = "Medicos")]
  public class HorarioMedicoVariavel : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }

    [Required]
    public Guid MedicoId { get; set; }

    [ForeignKey(nameof(MedicoId))]
    public Medico Medico { get; set; } = null!;

    [Required]
    public DateTime Data { get; set; }

    public TimeSpan? ManhaInicio { get; set; }
    public TimeSpan? ManhaFim { get; set; }
    public TimeSpan? TardeInicio { get; set; }
    public TimeSpan? TardeFim { get; set; }
  }
}
