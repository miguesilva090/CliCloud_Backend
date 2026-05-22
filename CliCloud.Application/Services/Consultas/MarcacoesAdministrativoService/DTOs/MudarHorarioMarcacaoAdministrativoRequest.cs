using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class MudarHorarioMarcacaoAdministrativoRequest : IDto
{
  public DateTime Data { get; set; }
  public TimeSpan HoraInicio { get; set; }
  public TimeSpan? HoraFim { get; set; }
}

public class MudarHorarioMarcacaoAdministrativoRequestValidator
  : AbstractValidator<MudarHorarioMarcacaoAdministrativoRequest>
{
  public MudarHorarioMarcacaoAdministrativoRequestValidator()
  {
    _ = RuleFor(x => x.Data).NotEmpty();
    _ = RuleFor(x => x.HoraInicio).NotEmpty();
    _ = RuleFor(x => x.HoraFim)
      .Must((dto, horaFim) => !horaFim.HasValue || horaFim > dto.HoraInicio)
      .WithMessage("HoraFim deve ser superior à HoraInicio.");
  }
}
