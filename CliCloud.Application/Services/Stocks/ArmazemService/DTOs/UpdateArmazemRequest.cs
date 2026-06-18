using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.ArmazemService.DTOs;

public class UpdateArmazemRequest : IDto
{
    public required string Nome { get; set; }
    public string? Morada { get; set; }
    public string? Localidade { get; set; }
    public Guid? CodigoPostalId { get; set; }
    public string? Telefone { get; set; }
    public string? Fax { get; set; }
    public bool ArmazemGeral { get; set; }
}

public class UpdateArmazemValidator : AbstractValidator<UpdateArmazemRequest>
{
    public UpdateArmazemValidator()
    {
        _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(40);
        _ = RuleFor(x => x.Morada).MaximumLength(50);
        _ = RuleFor(x => x.Localidade).MaximumLength(50);
        _ = RuleFor(x => x.Telefone).MaximumLength(20);
        _ = RuleFor(x => x.Fax).MaximumLength(20);
    }
}
