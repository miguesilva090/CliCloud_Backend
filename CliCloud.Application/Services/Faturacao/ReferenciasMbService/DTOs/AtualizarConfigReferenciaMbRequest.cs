using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Faturacao.ReferenciasMbService.DTOs;

public class AtualizarConfigReferenciaMbRequest : IDto
{
    public decimal ValorMinimo { get; set; }
    public int PrazoPagamento { get; set; }

    public string? ServicoUrl { get; set; }
    public string? CodigoEntidade { get; set; }
    public string? SubEntidade { get; set; }
    public string? ChaveBackOffice { get; set; }
    public string? IfThenKey { get; set; }
}

public class AtualizarConfigReferenciaMbValidator : AbstractValidator<AtualizarConfigReferenciaMbRequest>
{
    public AtualizarConfigReferenciaMbValidator()
    {
        _ = RuleFor(x => x.ValorMinimo).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.PrazoPagamento).GreaterThanOrEqualTo(0);
        _ = RuleFor(x => x.ServicoUrl).MaximumLength(255);

        _ = RuleFor(x => x.CodigoEntidade).NotEmpty();
        _ = RuleFor(x => x.SubEntidade).NotEmpty();
        _ = RuleFor(x => x.ChaveBackOffice).NotEmpty();
        _ = RuleFor(x => x.IfThenKey).NotEmpty();
    }
}