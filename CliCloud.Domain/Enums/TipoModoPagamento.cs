#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum TipoModoPagamento
  {
    [Display(Name = "Dinheiro")]
    Dinheiro = 1,
    
    [Display(Name = "Transferência Bancária")]
    TransferenciaBancaria = 2,
    
    [Display(Name = "Cheque")]
    Cheque = 3,
    
    [Display(Name = "Multibanco")]
    Multibanco = 4,
    
    [Display(Name = "Cartão de Crédito")]
    CartaoCredito = 5,
    
    [Display(Name = "Cartão de Débito")]
    CartaoDebito = 6,
    
    [Display(Name = "Débito Direto")]
    DebitoDireto = 7,
    
    [Display(Name = "Outro")]
    Outro = 8
  }
}
