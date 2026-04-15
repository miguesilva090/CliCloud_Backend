using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;

public class CreateSeparadorPersonalizadoRequest : IDto
{
    public string NomeSeparador { get; set; } = string.Empty;
    public Guid FormularioId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
}

public class CreateSeparadorPersonalizadoValidator : AbstractValidator<CreateSeparadorPersonalizadoRequest>
{
    public CreateSeparadorPersonalizadoValidator()
    {
        _ = RuleFor(x => x.NomeSeparador).NotEmpty().MaximumLength(150);
        _ = RuleFor(x => x.FormularioId).NotEmpty();
    }
}
