#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Credenciais
{
    [Table("TipoLotes", Schema = "Credenciais")]
    public class TipoLote : BaseEntity<int>
    {
        public int? Valor { get; set; }

        [StringLength(50)]
        public string? Designa { get; set; }
    }
}
