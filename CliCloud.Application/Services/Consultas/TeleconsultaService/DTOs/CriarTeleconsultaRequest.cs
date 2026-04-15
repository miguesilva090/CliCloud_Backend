using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.TeleconsultaService.DTOs
{
  public class CriarTeleconsultaRequest : IDto
  {
    public Guid ConsultaMarcacaoId { get; set; }
    public DateTime? InicioPrevistoUtc { get; set; }
    public int? DuracaoMinutos { get; set; }
  }

  public class CriarTeleconsultaRequestValidator : AbstractValidator<CriarTeleconsultaRequest>
  {
    public CriarTeleconsultaRequestValidator()
    {
      RuleFor(x => x.ConsultaMarcacaoId).NotEmpty();
      RuleFor(x => x.DuracaoMinutos).InclusiveBetween(5, 180).When(x => x.DuracaoMinutos.HasValue);
    }
  }
}
