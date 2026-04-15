using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoVinculoService.DTOs;

public class CreateSeparadorPersonalizadoVinculoRequest : IDto
{
    public Guid SeparadorPersonalizadoId { get; set; }
    public TipoVinculoSeparador Tipo { get; set; }
    public Guid EntidadeId { get; set; }
}

public class CreateSeparadorPersonalizadoVinculoValidator
    : AbstractValidator<CreateSeparadorPersonalizadoVinculoRequest>
{
    public CreateSeparadorPersonalizadoVinculoValidator()
    {
        _ = RuleFor(x => x.SeparadorPersonalizadoId).NotEmpty();
        _ = RuleFor(x => x.EntidadeId).NotEmpty();
        _ = RuleFor(x => x.Tipo).IsInEnum();
    }
}
