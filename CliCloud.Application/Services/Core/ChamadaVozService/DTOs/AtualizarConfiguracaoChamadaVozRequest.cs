using System;
using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Core.ChamadaVozService.DTOs
{
  public class AtualizarConfiguracaoChamadaVozRequest : IDto
  {
    public bool Ativo { get; set; }
    public string? Url { get; set; }
    public string? Language { get; set; }
    public string? Tld { get; set; }
  }

  public class AtualizarConfiguracaoChamadaVozValidator
    : AbstractValidator<AtualizarConfiguracaoChamadaVozRequest>
  {
    public AtualizarConfiguracaoChamadaVozValidator()
    {
      _ = RuleFor(x => x.Url).MaximumLength(500);
      _ = RuleFor(x => x.Language).MaximumLength(10);
      _ = RuleFor(x => x.Tld).MaximumLength(30);

      _ = RuleFor(x => x.Url)
        .NotEmpty()
        .When(x => x.Ativo)
        .WithMessage("URL é obrigatória quando a chamada de voz está ativa.");
    }
  }
}
