#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Tratamentos
{
    /// <summary>
    /// Estado da lista de espera (ex: Aguarda marcação definitiva, Em tentativa de contacto).
    /// </summary>
    [Table("EstadosListaEspera", Schema = "Tratamentos")]
    public class EstadoListaEspera : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Descricao { get; set; } = string.Empty;
    }
}
