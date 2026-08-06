using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Tratamentos.MarcacaoAutomaticaTratamentoService.DTOs;

public class MarcacaoAutomaticaPreviewRequest : IDto
{
    public Guid ListaEsperaTratamentoId { get; set; }
    public DateTime DataInicio { get; set; }
    public int NumeroSessoes { get; set; }
    public int IntervaloDias { get; set; } = 1;
    public List<int>? DiasSemanaPermitidos { get; set; }

    public Guid? FisioterapeutaId { get; set; }
    public int? UnidadeTempoFisio { get; set; }

    public Guid? AuxiliarId { get; set; }
    public int? UnidadeTempoAux { get; set; }

    public Guid? OutroTecnicoId { get; set; }
    public int? UnidadeTempoOutro { get; set; }
}

public class MarcacaoAutomaticaConfirmRequest : MarcacaoAutomaticaPreviewRequest
{
    public bool Provisorio { get; set; }
}

public class MarcacaoAutomaticaSessaoPreviewDTO : IDto
{
    public int NumSessao { get; set; }
    public DateTime Data { get; set; }
    public string HoraInic { get; set; } = string.Empty;
}

public class MarcacaoAutomaticaPreviewResponse : IDto 
{
    public Guid ListaEsperaTratamentoId { get; set; }
    public string UtenteNome { get; set; } = string.Empty;
    public int NumeroSessoesPedido { get; set; }
    public int NumeroSessoesGeradas { get; set; }
    public string? Duracao { get; set; }
    public IReadOnlyList<MarcacaoAutomaticaSessaoPreviewDTO> Sessoes { get; set; } = [];
}

public class MarcacaoAutomaticaPreviewValidator : AbstractValidator<MarcacaoAutomaticaPreviewRequest>
{
    public MarcacaoAutomaticaPreviewValidator()
    {
        _ = RuleFor(x => x.ListaEsperaTratamentoId).NotEmpty();
        _ = RuleFor(x => x.DataInicio).NotEmpty();
        _ = RuleFor(x => x.NumeroSessoes).InclusiveBetween(1, 200);
        _ = RuleFor(x => x.IntervaloDias).InclusiveBetween(1, 14);

        _ = RuleFor(x => x)
            .Must(x => x.FisioterapeutaId.HasValue || x.AuxiliarId.HasValue || x.OutroTecnicoId.HasValue)
            .WithMessage("Selecione pelo menos um técnico");

        _ = RuleFor(x => x.UnidadeTempoFisio)
            .InclusiveBetween(1, 50)
            .When(x => x.FisioterapeutaId.HasValue);

        _ = RuleFor(x => x.UnidadeTempoAux)
            .InclusiveBetween(1, 50)
            .When(x => x.AuxiliarId.HasValue);

        _ = RuleFor(x => x.UnidadeTempoOutro)
            .InclusiveBetween(1, 50)
            .When(x => x.OutroTecnicoId.HasValue);
    }
}

public class MarcacaoAutomaticaConfirmValidator : AbstractValidator<MarcacaoAutomaticaConfirmRequest>
{
  public MarcacaoAutomaticaConfirmValidator()
  {
    Include(new MarcacaoAutomaticaPreviewValidator());
  }
}