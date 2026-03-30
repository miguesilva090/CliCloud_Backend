using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.SubsistemaServicoService.DTOs
{
  public class CreateSubsistemaServicoRequest : IDto
  {
    public Guid ServicoId { get; set; }
    public Guid SubsistemaId { get; set; }
    public Guid OrganismoId { get; set; }

    public decimal ValorServico { get; set; }
    public decimal ValorOrganismo { get; set; }
    public decimal MargemOrganismoPercent { get; set; }
    public decimal ValorUtente { get; set; }
    public decimal MargemUtentePercent { get; set; }

    public bool Inativo { get; set; }
  }

  public class CreateSubsistemaServicoValidator : AbstractValidator<CreateSubsistemaServicoRequest>
  {
    public CreateSubsistemaServicoValidator()
    {
      _ = RuleFor(x => x.ServicoId).NotEmpty();
      _ = RuleFor(x => x.SubsistemaId).NotEmpty();
      _ = RuleFor(x => x.OrganismoId).NotEmpty();
      _ = RuleFor(x => x.ValorServico).GreaterThanOrEqualTo(0);
      _ = RuleFor(x => x.ValorOrganismo).GreaterThanOrEqualTo(0);
      _ = RuleFor(x => x.ValorUtente).GreaterThanOrEqualTo(0);
    }
  }
}
