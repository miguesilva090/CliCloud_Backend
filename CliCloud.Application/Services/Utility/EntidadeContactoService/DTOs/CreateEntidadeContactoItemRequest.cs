using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs
{
  public class CreateEntidadeContactoItemRequest : IDto
  {
    public required int EntidadeContactoTipoId { get; set; }
    public required string Valor { get; set; }
    public required bool Principal { get; set; }

  }

  public class CreateEntidadeContactoItemValidator : AbstractValidator<CreateEntidadeContactoItemRequest>
  {
    public CreateEntidadeContactoItemValidator()
    {
      _ = RuleFor(x => x.EntidadeContactoTipoId).NotEmpty();
      // Valor opcional: permite contacto sem valor preenchido (ex.: linha na UI para preencher depois)
      _ = RuleFor(x => x.Valor).NotNull();
    }
  }
}