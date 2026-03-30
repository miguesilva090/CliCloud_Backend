#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Exames
{
    [Table("CategoriaProcedimento", Schema = "Exames")]
    public class CategoriaProcedimento : AuditableEntity
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        [StringLength(100)]
        public string? Descricao {get;set;}

        public ICollection<TipoExame> TiposExame {get;set;} = new List<TipoExame>();
    }
}