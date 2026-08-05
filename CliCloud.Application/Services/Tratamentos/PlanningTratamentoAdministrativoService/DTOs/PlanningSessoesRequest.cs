using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PlanningTratamentoAdministrativoService.DTOs;

public class PlanningSessoesRequest : IDto
{
  public Guid TecnicoId { get; set; }
  /// <summary>1=Fisioterapeuta, 2=Auxiliar, 3=Outro.</summary>
  public int TipoTecnico { get; set; }
  public DateTime DataDe { get; set; }
  public DateTime DataAte { get; set; }
}

public class PlanningSessoesValidator : AbstractValidator<PlanningSessoesRequest>
{
  public PlanningSessoesValidator()
  {
    _ = RuleFor(x => x.TecnicoId).NotEmpty();
    _ = RuleFor(x => x.TipoTecnico).InclusiveBetween(1, 3);
    _ = RuleFor(x => x.DataAte)
      .GreaterThanOrEqualTo(x => x.DataDe.Date)
      .WithMessage("DataAte deve ser >= DataDe.");
    _ = RuleFor(x => x)
      .Must(x => (x.DataAte.Date - x.DataDe.Date).TotalDays <= 62)
      .WithMessage("Intervalo máximo de 62 dias.");
  }
}

public class PlanningSessoesResponse
{
  public List<PlanningSessaoEventoDTO> Eventos { get; set; } = [];
}
