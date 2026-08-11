using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs
{
  public class CreateReceitaLinhaRequest
  {
    public int Ordem { get; set; }
    public int TipoLinha { get; set; } = 1;
    public string? EmbId { get; set; }
    public string? Cnpem { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public string? DescricaoEmbalagem { get; set; }
    public int Quantidade { get; set; } = 1;
    public decimal? Pvp { get; set; }
    public decimal? Comparticipacao { get; set; }
    public decimal? ValorUtente { get; set; }
    public string? Posologia { get; set; }
    public string? PosologiaQuantidadeUnidade { get; set; }
    public string? PosologiaQuantidadeValor { get; set; }
    public string? PosologiaFrequenciaUnidade { get; set; }
    public string? PosologiaFrequenciaValor { get; set; }
    public string? PosologiaDuracaoUnidade { get; set; }
    public string? PosologiaDuracaoValor { get; set; }
    public string? PosologiaInstrucoes { get; set; }
    public int? CodValidade { get; set; }
    public string? CodJustificacaoQuantidade { get; set; }
    public string? JustificacaoQuantidade { get; set; }
    public int? CodTipoPrescricao { get; set; }
    public int? CodMotivo { get; set; }
    public int? CodIndicacaoTerapeutica { get; set; }
    public string? Diploma { get; set; }
  }

  public class CreateReceitaMedicaRequest : IDto
  {
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid ClinicaId { get; set; }
    public DateTime DataPrescricao { get; set; }
    public int TipoReceita { get; set; } = 1;
    public int Desmaterializada { get; set; } = 1;
    public int ReceitaRenovavel { get; set; }
    public int? NumeroVias { get; set; }
    public int PrescricaoPorNome { get; set; }
    public int? MotivoPrescricaoNome { get; set; }
    public string? NumeroBeneficiarioEfr { get; set; }
    public string? SiglaEfr { get; set; }
    public string? LocalPrescricao { get; set; }
    public string? Observacoes { get; set; }
    public List<CreateReceitaLinhaRequest> Linhas { get; set; } = [];
  }

  public class CreateReceitaMedicaValidator : AbstractValidator<CreateReceitaMedicaRequest>
  {
    public CreateReceitaMedicaValidator()
    {
      _ = RuleFor(x => x.UtenteId).NotEmpty();
      _ = RuleFor(x => x.MedicoId).NotEmpty();
      _ = RuleFor(x => x.DataPrescricao).NotEmpty()
        .WithMessage("Data de receita inválida.");
      _ = RuleFor(x => x.Linhas).Must(l => l != null && l.Count > 0)
        .WithMessage("Atenção, a receita tem de conter pelo menos um medicamento.");
      _ = RuleForEach(x => x.Linhas).ChildRules(linha =>
      {
        _ = linha.RuleFor(l => l.Designacao).NotEmpty().MaximumLength(500);
        _ = linha.RuleFor(l => l.Quantidade).GreaterThan(0);
      });
      _ = RuleFor(x => x)
        .Must(r => r.Linhas.All(l =>
          l.TipoLinha == 8
          || (!string.IsNullOrWhiteSpace(l.PosologiaQuantidadeUnidade)
              && !string.IsNullOrWhiteSpace(l.PosologiaQuantidadeValor)
              && !string.IsNullOrWhiteSpace(l.PosologiaFrequenciaUnidade)
              && !string.IsNullOrWhiteSpace(l.PosologiaFrequenciaValor))
          || !string.IsNullOrWhiteSpace(l.Posologia)))
        .WithMessage("Atenção, a POSOLOGIA é de preenchimento obrigatório.");
      _ = RuleFor(x => x.Linhas)
        .Must(linhas => linhas.All(l =>
          l.TipoLinha > 3
          || l.CodTipoPrescricao == 2
          || (l.CodMotivo is >= 1 and <= 4)))
        .WithMessage(
          "O Preenchimento do motivo da prescrição por medicamento é de caracter obrigatório");
      _ = RuleFor(x => x.Linhas)
        .Must(linhas => linhas.All(l =>
          l.TipoLinha != 2
          || (l.CodIndicacaoTerapeutica is >= 1 and <= 7)))
        .WithMessage(
          "O Preenchimento da indicação terapêutica é de caracter obrigatório");
    }
  }
}
