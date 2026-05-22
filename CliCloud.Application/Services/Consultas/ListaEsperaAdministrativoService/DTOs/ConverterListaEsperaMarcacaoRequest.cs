using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class ConverterListaEsperaMarcacaoRequest : IDto
{
  /// <summary>Data do slot na agenda; se null, usa a data do registo LE.</summary>
  public DateTime? DataMarcacao { get; set; }

  public TimeSpan HoraInicio { get; set; }
  public TimeSpan? HoraFim { get; set; }
  public Guid? TipoAdmissaoId { get; set; }
  public string? Obs { get; set; }

  /// <summary>
  /// true = mantém registo convertido na LE; false = remove da LE após criar marcação (legado).
  /// </summary>
  public bool ManterNaListaEspera { get; set; }
}

public class ConverterListaEsperaMarcacaoRequestValidator
  : AbstractValidator<ConverterListaEsperaMarcacaoRequest>
{
  public ConverterListaEsperaMarcacaoRequestValidator()
  {
    _ = RuleFor(x => x.HoraInicio).NotEmpty();
    _ = RuleFor(x => x.Obs).MaximumLength(2000);
  }
}
