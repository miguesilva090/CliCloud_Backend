#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Domain.Entities.Consultas
{
  [Table("Sala", Schema = "Consultas")]
  public class Sala : AuditableEntityWithSoftDelete
  {
    public string Nome { get; set; } = string.Empty;
    public int NumeroSala { get; set; }

    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    public bool Ativa { get; set; } = true;
  }
}