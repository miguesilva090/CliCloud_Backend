#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum TipoConsulta
  {
    [Display(Name = "Não Definido")]
    NaoDefinido = 0,
    
    [Display(Name = "Normal")]
    Normal = 1,
    
    [Display(Name = "Urgente")]
    Urgente = 2,
    
    [Display(Name = "Emergência")]
    Emergencia = 3
  }
}
