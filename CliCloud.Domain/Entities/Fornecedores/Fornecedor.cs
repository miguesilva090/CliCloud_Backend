#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Pagamentos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Enums;

namespace CliCloud.Domain.Entities.Fornecedores
{
  [Table("Fornecedor", Schema = "Fornecedores")]
  public class Fornecedor : Entidade
  {
    public Guid? InstituicaoFinanceiraId { get; set; }
    // public InstituicaoFinanceira? InstituicaoFinanceira { get; set; } // TODO: Criar entidade se necessário
    public string? NumeroConta { get; set; }
    public decimal? Plafond { get; set; }
    public Guid? CondicaoPagamentoId { get; set; }
    public CliCloud.Domain.Entities.Pagamentos.CondicaoPagamento? CondicaoPagamento { get; set; }
    public decimal? Desconto { get; set; }
    public Moeda? Moeda { get; set; }
    public decimal? TotalDebito { get; set; }
    public OrigemFornecedor? Origem { get; set; }
    public TipoFornecedor? TipoFornecedor { get; set; }
    public Guid? ModoPagamentoId { get; set; }
    public ModoPagamento? ModoPagamento { get; set; }
    public string? NumeroNib { get; set; }
    public int? Aprovado { get; set; }
    public DateOnly? DataAprovacao { get; set; }
    public string? EnderecoWeb { get; set; }
    public int? DiasPrevEntrega { get; set; }
    public int? DiasEfectiEntrega { get; set; }
  }
}
