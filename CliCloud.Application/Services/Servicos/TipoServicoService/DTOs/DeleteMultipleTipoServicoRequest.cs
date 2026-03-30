using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.DTOs
{
  public class DeleteMultipleTipoServicoRequest : IDto
  {
    public required IEnumerable<Guid> Ids { get; set; }
  }

  public class DeleteMultipleTipoServicoValidator : AbstractValidator<DeleteMultipleTipoServicoRequest>
  {
    public DeleteMultipleTipoServicoValidator()
    {
      _ = RuleFor(x => x.Ids)
        .NotEmpty()
        .WithMessage("A lista de IDs não pode estar vazia")
        .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
        .WithMessage("Todos os IDs devem ser GUIDs válidos");
    }
  }
}

