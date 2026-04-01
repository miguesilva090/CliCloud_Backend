#nullable enable 

using System.ComponentModel.DataAnnotations;

namespace CliCloud.Domain.Enums
{
    public enum ZonaFiscal
    {
        [Display(Name = "Continente")]
        Continente = 1,

        [Display(Name = "Madeira")]
        Madeira = 2,

        [Display(Name = "Açores")]
        Acores = 3,
    }
}