#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Domain.Entities.Tecnicos
{
  [Table("Tecnico", Schema = "Tecnicos")]
  public class Tecnico : EntidadePessoa
  {
    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }
    public double? Margem { get; set; }
    public Guid? IdUtilizador { get; set; }
    public HorarioTecnico? Horario { get; set; }
  }
}
