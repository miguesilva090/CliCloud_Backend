using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;

public class HistoricoTratamentoObservacoesDTO : IDto
{
  public string Observacoes { get; set; } = string.Empty;
}

public class AppendHistoricoTratamentoObservacaoRequest : IDto
{
  public string? Texto { get; set; }
}

public class AppendHistoricoTratamentoObservacaoRequestValidator
  : AbstractValidator<AppendHistoricoTratamentoObservacaoRequest>
{
  public AppendHistoricoTratamentoObservacaoRequestValidator()
  {
    RuleFor(x => x.Texto)
      .NotEmpty()
      .WithMessage("Indique o texto da observação.");
  }
}
