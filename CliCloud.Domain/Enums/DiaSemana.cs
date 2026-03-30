#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum DiaSemana
  {
    [Display(Name = "Domingo")]
    Domingo = 0,
    
    [Display(Name = "Segunda-feira")]
    Segunda = 1,
    
    [Display(Name = "Terça-feira")]
    Terca = 2,
    
    [Display(Name = "Quarta-feira")]
    Quarta = 3,
    
    [Display(Name = "Quinta-feira")]
    Quinta = 4,
    
    [Display(Name = "Sexta-feira")]
    Sexta = 5,
    
    [Display(Name = "Sábado")]
    Sabado = 6
  }
}
