using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs
{
  public class UpdateCartaConducaoRestricoesRequest : IDto
  {
    public int CodigoRestricao { get; set; }
    public string? Descricao { get; set; }
    public bool Inativo { get; set; }
  }

  public class UpdateCartaConducaoRestricoesValidator : AbstractValidator<UpdateCartaConducaoRestricoesRequest>
  {
    public UpdateCartaConducaoRestricoesValidator()
    {
      _ = RuleFor(x => x.CodigoRestricao).GreaterThanOrEqualTo(0).WithMessage("CodigoRestricao deve ser 0 ou superior.");
    }
  }
}
