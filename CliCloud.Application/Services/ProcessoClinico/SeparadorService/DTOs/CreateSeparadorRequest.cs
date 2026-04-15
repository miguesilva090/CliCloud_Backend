using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorService.DTOs;

public class CreateSeparadorRequest : IDto
{
    public string Nome { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
}

public class CreateSeparadorValidator : AbstractValidator<CreateSeparadorRequest>
{
    public CreateSeparadorValidator()
    {
        _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
    }
}
