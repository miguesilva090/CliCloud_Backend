using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.DTOs
{
    public class DeleteMultipleHabitosEViciosRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleHabitosEViciosValidator : AbstractValidator<DeleteMultipleHabitosEViciosRequest>
    {
        public DeleteMultipleHabitosEViciosValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}

