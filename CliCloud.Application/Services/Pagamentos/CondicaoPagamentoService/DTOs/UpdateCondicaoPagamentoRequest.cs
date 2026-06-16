using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Pagamentos.CondicaoPagamentoService.DTOs;

public class UpdateCondicaoPagamentoRequest : IDto
{
    public required string Descricao { get; set; }
    public int? NDiasPagamento { get; set; }
    public decimal? Desconto { get; set; }
}

public class UpdateCondicaoPagamentoValidator : AbstractValidator<UpdateCondicaoPagamentoRequest>
{
    public UpdateCondicaoPagamentoValidator()
    {
        _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(30);
    }
}
