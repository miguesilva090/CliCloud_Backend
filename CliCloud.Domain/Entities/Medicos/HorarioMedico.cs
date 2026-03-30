#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Medicos
{
  [Table("HorarioMedico", Schema = "Medicos")]
  public class HorarioMedico : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    public Guid MedicoId { get; set; }
    
    [ForeignKey(nameof(MedicoId))]
    public Medico Medico { get; set; } = null!;
    
    public int? TipoHorario { get; set; }
    public TimeSpan? MinMarcacao { get; set; }
    public bool HoraComp { get; set; }
    public TimeSpan? PrimeiraConsulta { get; set; }
    public bool HorarioFlexivel { get; set; }
    
    // Relacionamento 1:N com HorarioMedicoDia
    public ICollection<HorarioMedicoDia> Horarios { get; set; } = [];
  }
}
