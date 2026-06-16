using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;

public class CreateCondicaoPagamentoRequest : IDto
{
    public required string Descricao { get; set; }
    public int? NDiasPagamento { get; set; }
    public decimal? Desconto { get; set; }
}

public class CreateCondicaoPagamentoValidator : AbstractValidator<CreateCondicaoPagamentoRequest>
{
    public CreateCondicaoPagamentoValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(30);
    }
}
