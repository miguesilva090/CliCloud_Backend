#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tecnicos
{
  [Table("HorarioTecnico", Schema = "Tecnicos")]
  public class HorarioTecnico : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    public Guid TecnicoId { get; set; }
    
    [ForeignKey(nameof(TecnicoId))]
    public Tecnico Tecnico { get; set; } = null!;
    
    public int? TipoHorario { get; set; }
    public string? MinMarcacao { get; set; } 
    public int? HoraComp { get; set; }
    
    // Relacionamento 1:N com HorarioTecnicoDia
    public ICollection<HorarioTecnicoDia> Horarios { get; set; } = [];
  }
}
