using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs
{
  public class CreateTipoAparelhoRequest : IDto
  {
    public required string Designacao { get; set; }
  }

  public class CreateTipoAparelhoValidator : AbstractValidator<CreateTipoAparelhoRequest>
  {
    public CreateTipoAparelhoValidator()
    {
      _ = RuleFor(x => x.Designacao)
        .NotEmpty()
        .MaximumLength(100)
        .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
    }
  }
}
