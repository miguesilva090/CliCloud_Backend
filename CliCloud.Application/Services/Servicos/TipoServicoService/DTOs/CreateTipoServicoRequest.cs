using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.DTOs
{
  public class CreateTipoServicoRequest : IDto
  {
    public required string Descricao { get; set; }
    public int? Codigo { get; set; }
    public int? Filtro { get; set; }
    public decimal? TaxaModeradoraSns { get; set; }
    public bool PartilhaSemRequisicao { get; set; }
  }

  public class CreateTipoServicoValidator : AbstractValidator<CreateTipoServicoRequest>
  {
    public CreateTipoServicoValidator()
    {
      _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(80);
    }
  }
}

