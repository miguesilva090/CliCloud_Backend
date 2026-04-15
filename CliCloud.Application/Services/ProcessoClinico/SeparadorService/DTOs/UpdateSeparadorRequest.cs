using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;

public class UpdateSeparadorRequest : IDto
{
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
}

public class UpdateSeparadorValidator : AbstractValidator<UpdateSeparadorRequest>
{
    public UpdateSeparadorValidator()
    {
        _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
    }
}
