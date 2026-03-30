#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum OrigemFornecedor
  {
    [Display(Name = "Nacional")]
    Nacional = 1,
    
    [Display(Name = "Internacional")]
    Internacional = 2,
    
    [Display(Name = "Intracomunitário")]
    Intracomunitario = 3
  }
}
