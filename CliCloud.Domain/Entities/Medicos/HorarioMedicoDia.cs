#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Medicos
{
  [Table("HorarioMedicoDia", Schema = "Medicos")]
  public class HorarioMedicoDia : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    public Guid HorarioMedicoId { get; set; }
    
    [ForeignKey(nameof(HorarioMedicoId))]
    public HorarioMedico HorarioMedico { get; set; } = null!;
    
    [Required]
    public DiaSemana DiaSemana { get; set; }
    
    [Required]
    public Periodo Periodo { get; set; }
    
    public TimeSpan? Inicio { get; set; }
    public TimeSpan? Fim { get; set; }
    public string? Sala { get; set; }
    public int? Vagas { get; set; }
  }
}
