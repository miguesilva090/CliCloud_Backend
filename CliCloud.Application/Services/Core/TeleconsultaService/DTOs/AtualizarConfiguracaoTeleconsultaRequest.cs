using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Core.TeleconsultaService.DTOs
{
  public class AtualizarConfiguracaoTeleconsultaRequest : IDto
  {
    public bool Ativo { get; set; }
    public string? Provider { get; set; }
    public string? BaseMeetingUrl { get; set; }
    public bool JwtAtivo { get; set; }
    public string? JwtAppId { get; set; }
    public string? JwtApiKey { get; set; }
    public string? JwtKid { get; set; }
    public string? JwtPrivateKey { get; set; }
    public int JanelaEntradaMinutosAntes { get; set; }
    public int DuracaoPadraoMinutos { get; set; }
    public bool PermitirEntradaAntesDoInicio { get; set; }
    public bool LobbyAtivo { get; set; }
  }

  public class AtualizarConfiguracaoTeleconsultaValidator : AbstractValidator<AtualizarConfiguracaoTeleconsultaRequest>
  {
    public AtualizarConfiguracaoTeleconsultaValidator()
    {
      RuleFor(x => x.Provider).MaximumLength(50);
      RuleFor(x => x.BaseMeetingUrl).MaximumLength(300);
      RuleFor(x => x.JwtAppId).MaximumLength(120);
      RuleFor(x => x.JwtApiKey).MaximumLength(120);
      RuleFor(x => x.JwtKid).MaximumLength(120);
      RuleFor(x => x.JwtPrivateKey).MaximumLength(4000);

      RuleFor(x => x.JanelaEntradaMinutosAntes).InclusiveBetween(0, 180);
      RuleFor(x => x.DuracaoPadraoMinutos).InclusiveBetween(5, 360);

      RuleFor(x => x.BaseMeetingUrl)
        .NotEmpty()
        .When(x => x.Ativo)
        .WithMessage("URL base do meeting é obrigatória quando a teleconsulta está ativa.");

      RuleFor(x => x.BaseMeetingUrl)
        .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
        .When(x => !string.IsNullOrWhiteSpace(x.BaseMeetingUrl))
        .WithMessage("URL base do meeting inválida.");

      RuleFor(x => x.JwtAppId)
        .NotEmpty()
        .When(x => x.JwtAtivo)
        .WithMessage("JWT App ID é obrigatório quando JWT está ativo.");

      RuleFor(x => x.JwtApiKey)
        .NotEmpty()
        .When(x => x.JwtAtivo)
        .WithMessage("JWT API Key é obrigatória quando JWT está ativo.");

      RuleFor(x => x.JwtPrivateKey)
        .NotEmpty()
        .When(x => x.JwtAtivo)
        .WithMessage("JWT Private Key é obrigatória quando JWT está ativo.");
    }
  }
}
