using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs
{
  public class CreateCartaConducaoRequest : IDto
  {
    public string? CodigoCarta { get; set; }
    public string? Descricao { get; set; }
    public int Grupo { get; set; }
    public bool Inativo { get; set; }
  }

  public class CreateCartaConducaoValidator : AbstractValidator<CreateCartaConducaoRequest>
  {
    public CreateCartaConducaoValidator()
    {
      _ = RuleFor(x => x.Grupo).GreaterThanOrEqualTo(0).WithMessage("Grupo deve ser 0 ou superior.");
    }
  }
}
