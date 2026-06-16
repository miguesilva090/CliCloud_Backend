using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Pagamentos.ModoPagamentoService.DTOs;

public class CreateModoPagamentoRequest : IDto
{
    public required string Descricao { get; set; }
    public required string Abreviatura { get; set; }
    public bool TemNumAssociado { get; set; }
    public bool TemContaBancaria { get; set; }
    public Guid? ContaBancariaId { get; set; }
}

public class CreateModoPagamentoValidator : AbstractValidator<CreateModoPagamentoRequest>
{
    public CreateModoPagamentoValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(50);
        _ = RuleFor(x => x.Abreviatura).NotEmpty().MaximumLength(3);
    }
}
