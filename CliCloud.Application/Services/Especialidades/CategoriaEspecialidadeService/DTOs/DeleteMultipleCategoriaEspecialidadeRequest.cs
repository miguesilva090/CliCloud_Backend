using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.DTOs
{
    public class DeleteMultipleCategoriaEspecialidadeRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleCategoriaEspecialidadeValidator : AbstractValidator<DeleteMultipleCategoriaEspecialidadeRequest>
    {
        public DeleteMultipleCategoriaEspecialidadeValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}
