using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;


namespace CliCloud.Domain.Entities.ProcessoClinico.Odontologia
{
    [Table("EstadosDentarios", Schema = "Odontologia")]
    public class EstadosDentarios : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id {get;set;}

        [Required]
        [MaxLength(10)]
        public string Codigo {get;set;} = null!;

        [Required]
        [MaxLength(200)]
        public string Descricao {get;set;} = null!;

        public bool EstadoPadrao {get;set;}
        public bool Ativo {get;set;} = true;
    }
}