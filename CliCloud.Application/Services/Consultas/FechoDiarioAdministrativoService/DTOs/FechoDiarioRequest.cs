using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.FechoDiarioAdministrativoService.DTOs;

public class FechoDiarioRequest : IDto
{
  public DateTime? Data { get; set; }
}

public class FechoDiarioRequestValidator : AbstractValidator<FechoDiarioRequest>
{
  public FechoDiarioRequestValidator()
  {
    _ = RuleFor(x => x.Data).NotNull();
  }
}
