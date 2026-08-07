using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;

public class DesmarcarAdmissaoTratamentoRequest : IDto
{
  public required Guid MotivoDesmarcacaoId { get; set; }
}

public class DesmarcarAdmissaoTratamentoValidator
  : AbstractValidator<DesmarcarAdmissaoTratamentoRequest>
{
  public DesmarcarAdmissaoTratamentoValidator()
  {
    _ = RuleFor(x => x.MotivoDesmarcacaoId).NotEmpty()
      .WithMessage("Deve selecionar o motivo da desmarcação");
  }
}
