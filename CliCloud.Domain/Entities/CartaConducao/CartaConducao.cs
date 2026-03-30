#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.CartaConducao
{
    [Table("CartaConducao", Schema = "CartaConducao")]
    public class CartaConducao : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        public string? CodigoCarta { get; set; }
        public string? Descricao { get; set; }
        public int Grupo { get; set; }
        public bool Inativo { get; set; }
    }
}