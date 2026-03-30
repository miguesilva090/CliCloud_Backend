using System;
using System.Collections.Generic;
using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Artigos.GrupoViasAdministracaoService.DTOs
{
    public class DeleteMultipleGrupoViasAdministracaoRequest : IDto
    {
        public IEnumerable<Guid> Ids { get; set; } = Array.Empty<Guid>();
    }

    public class DeleteMultipleGrupoViasAdministracaoValidator
        : AbstractValidator<DeleteMultipleGrupoViasAdministracaoRequest>
    {
        public DeleteMultipleGrupoViasAdministracaoValidator()
        {
            _ = RuleFor(x => x.Ids)
                .NotNull()
                .Must(ids => ids.Any())
                .WithMessage("Deve indicar pelo menos um grupo de vias para eliminar.");
        }
    }
}

