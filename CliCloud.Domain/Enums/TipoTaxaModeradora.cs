#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum TipoTaxaModeradora
  {
    [Display(Name = "Isento")]
    Isento = 1,
    
    [Display(Name = "Não Isento")]
    NaoIsento = 2,
    
    [Display(Name = "E1111")]
    E1111 = 3,
    
    [Display(Name = "H")]
    H = 4,
    
    [Display(Name = "65")]
    SessentaCinco = 5
  }
}
