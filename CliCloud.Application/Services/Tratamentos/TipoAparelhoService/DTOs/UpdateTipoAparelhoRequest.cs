using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs
{
  public class UpdateTipoAparelhoRequest : IDto
  {
    public required string Designacao { get; set; }
  }

  public class UpdateTipoAparelhoValidator : AbstractValidator<UpdateTipoAparelhoRequest>
  {
    public UpdateTipoAparelhoValidator()
    {
      _ = RuleFor(x => x.Designacao)
        .NotEmpty()
        .MaximumLength(100)
        .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
    }
  }
}
