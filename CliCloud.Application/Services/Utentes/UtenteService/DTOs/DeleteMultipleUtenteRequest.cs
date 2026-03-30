using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtenteService.DTOs
{
  public class DeleteMultipleUtenteRequest : IDto
  {
    public required IEnumerable<Guid> Ids { get; set; }
  }

  public class DeleteMultipleUtenteValidator : AbstractValidator<DeleteMultipleUtenteRequest>
  {
    public DeleteMultipleUtenteValidator()
    {
      _ = RuleFor(x => x.Ids)
        .NotEmpty()
        .WithMessage("A lista de IDs não pode estar vazia")
        .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
        .WithMessage("Todos os IDs devem ser GUIDs válidos");
    }
  }
}
