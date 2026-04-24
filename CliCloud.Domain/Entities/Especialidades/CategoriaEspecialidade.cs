#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Especialidades
{
  [Table("CategoriaEspecialidade", Schema = "Especialidades")]
  public class CategoriaEspecialidade : AuditableEntityWithSoftDelete
  {
    [Key]
    public new Guid Id { get; set; }
    
    [Required]
    [StringLength(80)]
    public string Descricao { get; set; } = string.Empty;
    
    // Relacionamento 1:N com Especialidade
    public ICollection<Especialidade> Especialidades { get; set; } = [];
  }
}
