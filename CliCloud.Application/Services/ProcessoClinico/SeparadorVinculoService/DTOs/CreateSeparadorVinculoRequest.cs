using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorVinculoService.DTOs;

public class CreateSeparadorVinculoRequest : IDto
{
    public Guid SeparadorId { get; set; }
    public TipoVinculoSeparador Tipo { get; set; }
    public Guid EntidadeId { get; set; }
}

public class CreateSeparadorVinculoValidator : AbstractValidator<CreateSeparadorVinculoRequest>
{
    public CreateSeparadorVinculoValidator()
    {
        _ = RuleFor(x => x.SeparadorId).NotEmpty();
        _ = RuleFor(x => x.Tipo).IsInEnum();
        _ = RuleFor(x => x.EntidadeId).NotEmpty();
    }
}
