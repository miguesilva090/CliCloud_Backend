using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.DTOs;

public class UpdateSeparadorPersonalizadoRequest : IDto
{
    public string NomeSeparador { get; set; } = string.Empty;
    public Guid FormularioId { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
}

public class UpdateSeparadorPersonalizadoValidator : AbstractValidator<UpdateSeparadorPersonalizadoRequest>
{
    public UpdateSeparadorPersonalizadoValidator()
    {
        _ = RuleFor(x => x.NomeSeparador).NotEmpty().MaximumLength(150);
        _ = RuleFor(x => x.FormularioId).NotEmpty();
    }
}
