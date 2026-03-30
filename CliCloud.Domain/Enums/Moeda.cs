#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum Moeda
  {
    [Display(Name = "Euro")]
    EUR = 1,
    
    [Display(Name = "Dólar Americano")]
    USD = 2,
    
    [Display(Name = "Libra Esterlina")]
    GBP = 3,
    
    [Display(Name = "Franco Suíço")]
    CHF = 4,
    
    [Display(Name = "Iene Japonês")]
    JPY = 5
  }
}
