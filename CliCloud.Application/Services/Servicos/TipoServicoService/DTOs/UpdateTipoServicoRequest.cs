using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.DTOs
{
  public class UpdateTipoServicoRequest : IDto
  {
    public required string Descricao { get; set; }
    public decimal? TaxaModeradoraSns { get; set; }
    public bool PartilhaSemRequisicao { get; set; }
  }

  public class UpdateTipoServicoValidator : AbstractValidator<UpdateTipoServicoRequest>
  {
    public UpdateTipoServicoValidator()
    {
      _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(80);
    }
  }
}

