#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Domain.Entities.Exames
{
  /// <summary>
  /// Cabeçalho da prescrição de exames (modal "Prescrever Exames").
  /// Data, Prioridade, Nº Prescrição, Organismo, Observações; associado ao utente e médico.
  /// </summary>
  [Table("Exame", Schema = "Exames")]
  public class Exame : AuditableEntity
  {
    [Required]
    public Guid UtenteId { get; set; }
    [ForeignKey("UtenteId")]
    public Utente Utente { get; set; } = null!;

    [Required]
    public Guid MedicoId { get; set; }
    [ForeignKey("MedicoId")]
    public Medico Medico { get; set; } = null!;

    [Required]
    public DateTime DataPrescricao { get; set; }

    public Guid? PrioridadeId { get; set; }
    [ForeignKey("PrioridadeId")]
    public Prioridade? Prioridade { get; set; }

    [StringLength(50)]
    public string? NumeroPrescricao { get; set; }

    public Guid? OrganismoId { get; set; }
    [ForeignKey("OrganismoId")]
    public Organismo? Organismo { get; set; }

    public string? Observacoes { get; set; }

    public ICollection<ExameLinha> Linhas { get; set; } = new List<ExameLinha>();
  }
}
