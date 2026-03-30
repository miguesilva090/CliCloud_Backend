using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs
{
  public class UpdateEntidadeContactoItemRequest : IDto
  {
    public required Guid Id { get; set; }
    public required string EntidadeContactoTipoId { get; set; }
    public required string Valor { get; set; }
    public required bool Principal { get; set; }

  }

  public class UpdateEntidadeContactoItemValidator : AbstractValidator<UpdateEntidadeContactoItemRequest>
  {
    public UpdateEntidadeContactoItemValidator()
    {
      _ = RuleFor(x => x.Id).NotEmpty();
      _ = RuleFor(x => x.EntidadeContactoTipoId).NotEmpty();
      _ = RuleFor(x => x.Valor).NotEmpty();
    }
  }
}