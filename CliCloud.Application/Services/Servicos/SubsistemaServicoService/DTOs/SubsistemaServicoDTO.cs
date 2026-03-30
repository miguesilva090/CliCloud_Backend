using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs
{
  public class SubsistemaServicoDTO : IDto
  {
    public Guid Id { get; set; }

    public Guid ServicoId { get; set; }
    public Guid SubsistemaId { get; set; }
    public Guid OrganismoId { get; set; }

    public decimal ValorServico { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorUtente { get; set; }
    public decimal MargemUtentePercent { get; set; }

    public bool Inativo { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}

