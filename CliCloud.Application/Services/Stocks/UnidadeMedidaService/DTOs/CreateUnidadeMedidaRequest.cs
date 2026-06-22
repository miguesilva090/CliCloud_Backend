using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;

public class CreateUnidadeMedidaRequest : IDto
{
    public required string Descricao { get; set; }
}

public class CreateUnidadeMedidaValidator : AbstractValidator<CreateUnidadeMedidaRequest>
{
    public CreateUnidadeMedidaValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(15);
    }
}
