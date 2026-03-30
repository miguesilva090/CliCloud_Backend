using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.DTOs
{
    public class DeleteMultipleMotivosDesmarcacaoRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleMotivosDesmarcacaoValidator : AbstractValidator<DeleteMultipleMotivosDesmarcacaoRequest>
    {
        public DeleteMultipleMotivosDesmarcacaoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}