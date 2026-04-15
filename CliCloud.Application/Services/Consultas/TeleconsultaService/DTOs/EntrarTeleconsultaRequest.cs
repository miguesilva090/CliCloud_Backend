using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs
{
  public class EntrarTeleconsultaRequest : IDto
  {
    public string Papel { get; set; } = "Profissional";
    public string? NomeExibicao { get; set; }
    public string? CodigoAcesso { get; set; }
  }

  public class EntrarTeleconsultaRequestValidator : AbstractValidator<EntrarTeleconsultaRequest>
  {
    public EntrarTeleconsultaRequestValidator()
    {
      RuleFor(x => x.Papel).NotEmpty().MaximumLength(30);
      RuleFor(x => x.NomeExibicao).MaximumLength(120);
      RuleFor(x => x.CodigoAcesso).MaximumLength(200);
    }
  }
}
