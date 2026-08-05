#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Especialidades;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Tecnicos
{
  [Table("Tecnico", Schema = "Tecnicos")]
  public class Tecnico : EntidadePessoa
  {
    public Guid? EspecialidadeId { get; set; }
    public Especialidade? Especialidade { get; set; }
    public double? Margem { get; set; }
    public Guid? IdUtilizador { get; set; }
    /// <summary>Papel em tratamentos: Fisioterapeuta / Auxiliar / Outro.</summary>
    public TipoTecnico TipoTecnico { get; set; } = TipoTecnico.Fisioterapeuta;
    public int MaxTratamentos { get; set; } = 1;
    public HorarioTecnico? Horario { get; set; }
  }
}
