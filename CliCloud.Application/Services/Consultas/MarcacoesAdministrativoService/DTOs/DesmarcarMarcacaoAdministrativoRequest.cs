using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class DesmarcarMarcacaoAdministrativoRequest : IDto
{
  public string Motivo { get; set; } = string.Empty;
}

public class DesmarcarMarcacaoAdministrativoRequestValidator
  : AbstractValidator<DesmarcarMarcacaoAdministrativoRequest>
{
  public DesmarcarMarcacaoAdministrativoRequestValidator()
  {
    _ = RuleFor(x => x.Motivo)
      .NotEmpty()
      .MaximumLength(500);
  }
}
