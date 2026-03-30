using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.FraquezasMuscularesService.DTOs
{
    public class DeleteMultipleFraquezasMuscularesRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleFraquezasMuscularesValidator : AbstractValidator<DeleteMultipleFraquezasMuscularesRequest>
    {
        public DeleteMultipleFraquezasMuscularesValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}