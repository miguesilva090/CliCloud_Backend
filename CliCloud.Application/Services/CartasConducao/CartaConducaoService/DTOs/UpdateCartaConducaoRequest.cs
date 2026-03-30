using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs
{
  public class UpdateCartaConducaoRequest : IDto
  {
    public string? CodigoCarta { get; set; }
    public string? Descricao { get; set; }
    public int Grupo { get; set; }
    public bool Inativo { get; set; }
  }

  public class UpdateCartaConducaoValidator : AbstractValidator<UpdateCartaConducaoRequest>
  {
    public UpdateCartaConducaoValidator()
    {
      _ = RuleFor(x => x.Grupo).GreaterThanOrEqualTo(0).WithMessage("Grupo deve ser 0 ou superior.");
    }
  }
}
