using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ClinicaService.DTOs
{
  public class CreateClinicaRequest : IDto
  {
    public required string Nome { get; set; }
    public string? NomeComercial { get; set; }
    public string? Abreviatura { get; set; }
  }

  public class CreateClinicaValidator : AbstractValidator<CreateClinicaRequest>
  {
    public CreateClinicaValidator()
    {
      _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
      _ = RuleFor(x => x.NomeComercial).MaximumLength(100);
      _ = RuleFor(x => x.Abreviatura).MaximumLength(40);
    }
  }
}
