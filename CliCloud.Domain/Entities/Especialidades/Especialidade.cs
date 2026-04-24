#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Especialidades
{
  [Table("Especialidade", Schema = "Especialidades")]
  public class Especialidade : AuditableEntityWithSoftDelete
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    [StringLength(30)]
    public string Nome { get; set; } = string.Empty;
    
    public Guid? CategoriaEspecialidadeId { get; set; }
    
    [ForeignKey(nameof(CategoriaEspecialidadeId))]
    public CategoriaEspecialidade? CategoriaEspecialidade { get; set; }
    
    public bool Fisioterapia { get; set; }
    
    public bool Atendimento { get; set; }
    
    public bool Globalbooking { get; set; }
  }
}
