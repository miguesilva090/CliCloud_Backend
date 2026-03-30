using FluentValidation;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs
{
    public class DeleteMultipleRegiaoCorpoRequest 
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleRegiaoCorpoValidator : AbstractValidator<DeleteMultipleRegiaoCorpoRequest>
    {
        public DeleteMultipleRegiaoCorpoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}