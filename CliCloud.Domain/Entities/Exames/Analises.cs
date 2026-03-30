#nullable enable 

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Exames
{
    [Table("Analises", Schema = "Exames")]
    public class Analises : AuditableEntity
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        [StringLength(200)]
        public string? Nome {get;set;}

        [StringLength(50)]
        public string? UnidadeMedida {get;set;}

        [StringLength(200)]
        public string? ValoresReferencia {get;set;}

    }
}