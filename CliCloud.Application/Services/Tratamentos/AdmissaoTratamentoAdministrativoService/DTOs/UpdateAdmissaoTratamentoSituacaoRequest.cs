using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;

public class UpdateAdmissaoTratamentoSituacaoRequest : IDto
{
  /// <summary>confirmado | efetuado | faltou</summary>
  public required string Campo { get; set; }
  public required int Valor { get; set; }
}

public class UpdateAdmissaoTratamentoSituacaoValidator
  : AbstractValidator<UpdateAdmissaoTratamentoSituacaoRequest>
{
  public UpdateAdmissaoTratamentoSituacaoValidator()
  {
    _ = RuleFor(x => x.Campo)
      .NotEmpty()
      .Must(c =>
        c.Equals("confirmado", StringComparison.OrdinalIgnoreCase)
        || c.Equals("efetuado", StringComparison.OrdinalIgnoreCase)
        || c.Equals("faltou", StringComparison.OrdinalIgnoreCase)
      )
      .WithMessage("Campo deve ser confirmado, efetuado ou faltou.");
    _ = RuleFor(x => x.Valor).InclusiveBetween(0, 1);
  }
}
