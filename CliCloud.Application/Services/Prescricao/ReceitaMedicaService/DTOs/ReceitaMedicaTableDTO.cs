using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.DTOs
{
  public class ReceitaLinhaDTO : IDto
  {
    public Guid Id { get; set; }
    public int Ordem { get; set; }
    public int TipoLinha { get; set; }
    public string? EmbId { get; set; }
    public string? Cnpem { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public string? DescricaoEmbalagem { get; set; }
    public int Quantidade { get; set; }
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
    public DateTime? DataValidade { get; set; }
    public string? CodJustificacaoQuantidade { get; set; }
    public string? JustificacaoQuantidade { get; set; }
    public int? CodTipoPrescricao { get; set; }
    public int? CodMotivo { get; set; }
    public int? CodIndicacaoTerapeutica { get; set; }
    public string? Diploma { get; set; }
  }

  public class ReceitaMedicaDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid ClinicaId { get; set; }
    public DateTime DataPrescricao { get; set; }
    public int TipoReceita { get; set; }
    public int Desmaterializada { get; set; }
    public string? NumeroReceitaLocal { get; set; }
    public string? NumeroReceita { get; set; }
    public int Enviada { get; set; }
    public int Anulada { get; set; }
    public int ReceitaRenovavel { get; set; }
    public int? NumeroVias { get; set; }
    public int PrescricaoPorNome { get; set; }
    public int? MotivoPrescricaoNome { get; set; }
    public string? NumeroBeneficiarioEfr { get; set; }
    public string? SiglaEfr { get; set; }
    public string? LocalPrescricao { get; set; }
    public string? Observacoes { get; set; }
    public int EstadoEnvio { get; set; }
    public string? MensagemErro { get; set; }
    public List<ReceitaLinhaDTO> Linhas { get; set; } = [];
  }

  public class ReceitaMedicaTableDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime DataPrescricao { get; set; }
    public string? NumeroReceitaLocal { get; set; }
    public string? NumeroReceita { get; set; }
    public Guid UtenteId { get; set; }
    public string? UtenteNome { get; set; }
    public Guid MedicoId { get; set; }
    public string? MedicoNome { get; set; }
    public int TipoReceita { get; set; }
    public int Desmaterializada { get; set; }
    public int Enviada { get; set; }
    public int Anulada { get; set; }
    public int EstadoEnvio { get; set; }
  }
}
