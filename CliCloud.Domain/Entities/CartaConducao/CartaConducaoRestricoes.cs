#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.CartaConducao
{
    [Table("CartaConducaoRestricoes", Schema = "CartaConducao")]
    public class CartaConducaoRestricao : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }
        public int CodigoRestricao { get; set; }
        public string? Descricao { get; set; }
        public bool Inativo { get; set; }
    }
}
