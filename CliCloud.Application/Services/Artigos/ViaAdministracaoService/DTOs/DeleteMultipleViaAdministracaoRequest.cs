using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Artigos.ViaAdministracaoService.DTOs
{
    public class DeleteMultipleViaAdministracaoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = Array.Empty<Guid>();
    }

    public class DeleteMultipleViaAdministracaoValidator : AbstractValidator<DeleteMultipleViaAdministracaoRequest>
    {
        public DeleteMultipleViaAdministracaoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotNull()
                .Must(ids => ids.Any())
                .WithMessage("Deve indicar pelo menos uma via de administração para eliminar.");
        }
    }
}