#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum CondicaoPagamento
  {
    [Display(Name = "À Vista")]
    AVista = 1,
    
    [Display(Name = "30 Dias")]
    TrintaDias = 2,
    
    [Display(Name = "60 Dias")]
    SessentaDias = 3,
    
    [Display(Name = "90 Dias")]
    NoventaDias = 4,
    
    [Display(Name = "120 Dias")]
    CentoVinteDias = 5,
    
    [Display(Name = "Pré-pagamento")]
    PrePagamento = 6,
    
    [Display(Name = "Outro")]
    Outro = 7
  }
}
