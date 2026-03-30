#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum Periodo
  {
    [Display(Name = "Manhã")]
    Manha = 0,
    
    [Display(Name = "Tarde")]
    Tarde = 1
  }
}
