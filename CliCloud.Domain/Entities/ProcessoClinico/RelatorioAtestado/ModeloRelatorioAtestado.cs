using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado
{
    [Table("ModelosRelatorioAtestado", Schema = "ProcessoClinico")]
    public class ModeloRelatorioAtestado : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        public Guid EmpresaId { get; set; }

        // null = modelo global; valor = modelo específico do médico
        public Guid? MedicoId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = null!;

        [Required]
        public string TextoHtml { get; set; } = null!;
    }
}

