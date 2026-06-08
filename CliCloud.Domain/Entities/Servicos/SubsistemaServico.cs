using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Servicos
{
 
  [Table("SubsistemaServico", Schema = "Servicos")]
  public class SubsistemaServico : AuditableEntityWithSoftDelete
  {
    public Guid ServicoId { get; set; }
    public Servico Servico { get; set; } = null!;

    public Guid SubsistemaId { get; set; }

    public Guid OrganismoId { get; set; }

    public decimal ValorServico { get; set; }

    public decimal ValorOrganismo { get; set; }

    public decimal MargemOrganismoPercent { get; set; }

    public decimal ValorUtente { get; set; }

    public decimal MargemUtentePercent { get; set; }

    public bool Inativo { get; set; }
  }
}
