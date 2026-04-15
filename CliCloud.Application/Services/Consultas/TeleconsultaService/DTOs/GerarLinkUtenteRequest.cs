using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs
{
  public class GerarLinkUtenteRequest : IDto
  {
    public string? NomeExibicao { get; set; }
    public string? Destino { get; set; }
  }

  public class GerarLinkUtenteRequestValidator : AbstractValidator<GerarLinkUtenteRequest>
  {
    public GerarLinkUtenteRequestValidator()
    {
      RuleFor(x => x.NomeExibicao).MaximumLength(120);
      RuleFor(x => x.Destino).MaximumLength(120);
    }
  }
}
