using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs
{
  public class CreateCartaConducaoRestricoesRequest : IDto
  {
    public int CodigoRestricao { get; set; }
    public string? Descricao { get; set; }
    public bool Inativo { get; set; }
  }

  public class CreateCartaConducaoRestricoesValidator : AbstractValidator<CreateCartaConducaoRestricoesRequest>
  {
    public CreateCartaConducaoRestricoesValidator()
    {
      _ = RuleFor(x => x.CodigoRestricao).GreaterThanOrEqualTo(0).WithMessage("CodigoRestricao deve ser 0 ou superior.");
    }
  }
}
