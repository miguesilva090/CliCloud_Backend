#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum StatusValidacao
  {
    [Display(Name = "Não Validado")]
    NaoValidado = 0,
    
    [Display(Name = "Validado")]
    Validado = 1
  }
}
