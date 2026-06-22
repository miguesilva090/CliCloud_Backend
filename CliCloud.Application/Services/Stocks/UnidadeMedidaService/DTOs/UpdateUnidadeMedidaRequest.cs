using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.UnidadeMedidaService.DTOs;

public class UpdateUnidadeMedidaRequest : IDto
{
    public required string Descricao { get; set; }
}

public class UpdateUnidadeMedidaValidator : AbstractValidator<UpdateUnidadeMedidaRequest>
{
    public UpdateUnidadeMedidaValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(15);
    }
}
