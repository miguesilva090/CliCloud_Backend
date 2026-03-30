using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
    public class DeleteMultipleDocumentoRequest : IDto
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }

    public class DeleteMultipleDocumentoValidator : AbstractValidator<DeleteMultipleDocumentoRequest>
    {
        public DeleteMultipleDocumentoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("A lista de IDs não pode estar vazia")
                .Must(ids => ids != null && ids.All(id => id != Guid.Empty))
                .WithMessage("Todos os IDs devem ser GUIDs válidos");
        }
    }
}
