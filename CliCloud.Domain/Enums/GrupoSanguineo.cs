using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum GrupoSanguineo
  {
    [Display(Name = "A+")]
    APositivo,
    
    [Display(Name = "A-")]
    ANegativo,
    
    [Display(Name = "B+")]
    BPositivo,
    
    [Display(Name = "B-")]
    BNegativo,
    
    [Display(Name = "AB+")]
    ABPositivo,
    
    [Display(Name = "AB-")]
    ABNegativo,
    
    [Display(Name = "O+")]
    OPositivo,
    
    [Display(Name = "O-")]
    ONegativo,
    
    [Display(Name = "Não Definido")]
    NaoDefinido
  }
}