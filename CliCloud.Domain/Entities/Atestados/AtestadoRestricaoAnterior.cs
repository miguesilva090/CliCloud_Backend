#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.CartaConducao;

namespace CliCloud.Domain.Entities.Atestados
{
    [Table("AtestadoRestricaoAnterior", Schema = "Atestados")]
    public class AtestadoRestricaoAnterior : AuditableEntityWithSoftDelete
    {
        public Guid AtestadoId { get; set; }
        public Atestado? Atestado { get; set; }

        public Guid CartaConducaoRestricaoId { get; set; }
        public CartaConducaoRestricao? CartaConducaoRestricao { get; set; }

        public string? Anotacoes { get; set; }
    }
}