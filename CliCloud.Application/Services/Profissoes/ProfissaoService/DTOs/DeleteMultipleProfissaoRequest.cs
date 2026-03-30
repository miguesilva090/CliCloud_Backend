using FluentValidation;

namespace CliCloud.Application.Services.Profissoes.ProfissaoService.DTOs
{
    public class DeleteMultipleProfissaoRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleProfissaoValidator : AbstractValidator<DeleteMultipleProfissaoRequest>
    {
        public DeleteMultipleProfissaoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}
