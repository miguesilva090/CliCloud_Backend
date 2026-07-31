#nullable enable

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
  /// <summary>
  /// Papel do técnico em tratamentos (paridade legado TERAPEUTA / AUXILIAR / TERAPEUTAOCUP).
  /// </summary>
  public enum TipoTecnico
  {
    [Display(Name = "Fisioterapeuta")]
    Fisioterapeuta = 1,

    [Display(Name = "Auxiliar")]
    Auxiliar = 2,

    [Display(Name = "Terapeuta Ocupacional/Fala")]
    Outro = 3
  }
}
