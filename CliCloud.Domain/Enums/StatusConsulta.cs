#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  public enum StatusConsulta
  {
    [Display(Name = "Agendada")]
    Agendada = 0,
    
    [Display(Name = "Pendente")]
    Pendente = 1,

    [Display(Name = "Desmarcada")]
    Desmarcada = 2,

    [Display(Name = "Suspensa")]
    Suspensa = 4,

    [Display(Name = "Em atendimento")]
    EmAtendimento = 5,

    [Display(Name = "Concluída")]
    Concluida = 6,

    [Display(Name = "Faltou")]
    Faltou = 7,

    [Display(Name = "Faltou (Justificada)")]
    FaltouJustificada = 8
  }
}