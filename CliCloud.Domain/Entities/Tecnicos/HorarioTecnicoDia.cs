#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Tecnicos
{
  [Table("HorarioTecnicoDia", Schema = "Tecnicos")]
  public class HorarioTecnicoDia : AuditableEntity
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    public Guid HorarioTecnicoId { get; set; }
    
    [ForeignKey(nameof(HorarioTecnicoId))]
    public HorarioTecnico HorarioTecnico { get; set; } = null!;
    
    [Required]
    public DiaSemana DiaSemana { get; set; }
    
    [Required]
    public Periodo Periodo { get; set; }
    
    public string? Inicio { get; set; } 
    public string? Fim { get; set; } 
    public string? Sala { get; set; }
    public int? NumMarcacoesPeriodo { get; set; }
    public int? NumMarcacoesOutro { get; set; }
  }
}
