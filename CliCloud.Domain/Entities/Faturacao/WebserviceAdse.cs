#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Faturacao;

[Table("WebserviceAdse", Schema = "Faturacao")]
public class WebserviceAdse : AuditableEntityWithSoftDelete
{
    public new Guid Id { get; set; }

    public Guid ClinicaId { get; set; }

    public Guid OrganismoId { get; set; }

    /// <summary>Clínica fisioterapia associada à configuração ADSE.</summary>
    public Guid ClinicaFisioterapiaId { get; set; }

    [StringLength(254)]
    public string UrlAdse { get; set; } = string.Empty;

    [StringLength(20)]
    public string DominioUserAdse { get; set; } = string.Empty;

    [StringLength(20)]
    public string UserAdse { get; set; } = string.Empty;

    [StringLength(20)]
    public string PasswordAdse { get; set; } = string.Empty;

    public int NumlocalAdse { get; set; }

    [StringLength(200)]
    public string? NomelocalAdse { get; set; }

    [StringLength(50)]
    public string PasslocalAdse { get; set; } = string.Empty;

    [StringLength(200)]
    public string PastaPdfAdse { get; set; } = string.Empty;
}