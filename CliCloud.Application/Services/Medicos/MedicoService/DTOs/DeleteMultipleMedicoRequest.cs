using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.MedicoService.DTOs
{
  public class DeleteMultipleMedicoRequest : IDto
  {
    public required IEnumerable<Guid> Ids { get; set; }
  }

  public class DeleteMultipleMedicoValidator : AbstractValidator<DeleteMultipleMedicoRequest>
  {
    public DeleteMultipleMedicoValidator()
    {
      _ = RuleFor(x => x.Ids)
        .NotEmpty()
        .WithMessage("A lista de IDs não pode estar vazia")
        .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
        .WithMessage("Todos os IDs devem ser GUIDs válidos");
    }
  }
}
