using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Core.ConfigReferenciaMB;

[Table("ConfigReferenciaMB", Schema = "Core")]
public class ConfigReferenciaMB : AuditableEntityWithSoftDelete
{
    public Guid ClinicaId { get; set; }

    public decimal ValorMinimo { get; set; }
    public int PrazoPagamento { get; set; }

    public string? ServicoUrl { get; set; }
    public string? CodigoEntidade { get; set; }
    public string? SubEntidade { get; set; }
    public string? ChaveBackOffice { get; set; }
    public string? IfThenKey { get; set; }

}