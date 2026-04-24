#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Empresas;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Domain.Entities.Utentes
{
  /// <summary>Linha do subsistema de saúde do utente (organismo, beneficiário, apólice, etc.).</summary>
  [Table("UtenteSubsistemaLinha", Schema = "Utentes")]
  public class UtenteSubsistemaLinha : AuditableEntityWithSoftDelete
  {
    public Guid UtenteId { get; set; }
    public Utente? Utente { get; set; }

    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }

    public string? Designacao { get; set; }
    public string? NumeroBeneficiario { get; set; }
    public string? Sigla { get; set; }
    public string? NomeBeneficiario { get; set; }
    public DateOnly? DataCartao { get; set; }
    public string? NumeroApolice { get; set; }

    public Guid? EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
  }
}
