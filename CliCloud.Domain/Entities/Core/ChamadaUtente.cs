#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core
{
  [Table("ChamadaUtente", Schema = "Core")]
  public class ChamadaUtente : AuditableEntityWithSoftDelete
  {
    public Guid ClinicaId { get; set; }
    public Clinica Clinica { get; set; } = null!;

    [StringLength(20)]
    public string Tipo { get; set; } = null!; // Consulta | Tratamento

    public Guid ReferenciaId { get; set; } // ConsultaMarcacaoId ou SessaoTratamentoId
    public Guid? UtenteId { get; set; }

    [StringLength(200)]
    public string NomeUtente { get; set; } = string.Empty;

    [StringLength(200)]
    public string? NomeProfissional { get; set; }

    [StringLength(50)]
    public string? Sala { get; set; }

    [StringLength(50)]
    public string? Senha { get; set; }

    public DateTime DataHoraChamada { get; set; }

    public int Estado { get; set; } // 0=ativa/nova; 1=processada
  }
}
