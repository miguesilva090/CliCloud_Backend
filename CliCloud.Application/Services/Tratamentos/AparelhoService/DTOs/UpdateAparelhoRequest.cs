using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs
{
  public class UpdateAparelhoRequest : IDto
  {
    public required string TipoAparelhoId { get; set; }
    public string? ModeloAparelhoId { get; set; }
    public string? CodigoSerie { get; set; }
    public string? CodigoInventario { get; set; }
    public string? Local { get; set; }
    public string? Observacoes { get; set; }
    public bool Ocupado { get; set; }
  }

  public class UpdateAparelhoValidator : AbstractValidator<UpdateAparelhoRequest>
  {
    public UpdateAparelhoValidator()
    {
      _ = RuleFor(x => x.TipoAparelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("TipoAparelhoId inválido.");
      _ = RuleFor(x => x.ModeloAparelhoId).Must(s => string.IsNullOrEmpty(s) || GSHelpers.BeValidGuid(s)).WithMessage("ModeloAparelhoId inválido.");
      _ = RuleFor(x => x.CodigoSerie).MaximumLength(50);
      _ = RuleFor(x => x.CodigoInventario).MaximumLength(50);
      _ = RuleFor(x => x.Local).MaximumLength(100);
      _ = RuleFor(x => x.Observacoes).MaximumLength(500);
    }
  }
}
