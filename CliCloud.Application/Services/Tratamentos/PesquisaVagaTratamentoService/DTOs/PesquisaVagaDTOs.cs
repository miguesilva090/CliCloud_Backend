using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Tratamentos.PesquisaVagaTratamentoService.DTOs;

public class PesquisaVagaRequest : IDto
{
  public DateTime DataInicio { get; set; }
  public string Hora { get; set; } = string.Empty;
  public int NumeroSessoes { get; set; }
  public int UnidadeTempo { get; set; } = 1;

  public bool DiasConsecutivos { get; set; } = true;
  public List<int>? DiasSemana { get; set; }
  public List<int>? TiposTecnico { get; set; }
}

public class PesquisaVagaTecnicoDTO : IDto
{
  public Guid TecnicoId { get; set; }
  public string Nome { get; set; } = string.Empty;
  public int TipoTecnico { get; set; }
}

public class PesquisaVagaResponse : IDto
{
  public IReadOnlyList<PesquisaVagaTecnicoDTO> Fisioterapeutas { get; set; } = [];
  public IReadOnlyList<PesquisaVagaTecnicoDTO> Auxiliares { get; set; } = [];
  public IReadOnlyList<PesquisaVagaTecnicoDTO> Outros { get; set; } = [];
}

public class PesquisaVagaRequestValidator : AbstractValidator<PesquisaVagaRequest>
{
  public PesquisaVagaRequestValidator()
  {
    _ = RuleFor(x => x.DataInicio).NotEmpty();
    _ = RuleFor(x => x.Hora).NotEmpty().WithMessage("Indique a hora.");
    _ = RuleFor(x => x.NumeroSessoes).InclusiveBetween(1, 200);
    _ = RuleFor(x => x.UnidadeTempo).InclusiveBetween(1, 50);

    _ = RuleFor(x => x.DiasSemana)
      .Must(d => d != null && d.Count > 0)
      .When(x => !x.DiasConsecutivos)
      .WithMessage("Tem de selecionar os dias");

    _ = RuleForEach(x => x.DiasSemana!)
      .InclusiveBetween(0, 6)
      .When(x => x.DiasSemana != null);
  }
}
