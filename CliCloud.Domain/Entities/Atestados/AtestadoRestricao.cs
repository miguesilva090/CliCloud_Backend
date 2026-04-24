#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.CartaConducao;
using CliCloud.Domain.Entities.Common;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;

namespace CliCloud.Domain.Entities.Atestados
{
    [Table("AtestadoRestricao", Schema = "Atestados")]
    public class AtestadoRestricao : AuditableEntityWithSoftDelete
    {
        public Guid AtestadoId { get; set; }
        public Atestado? Atestado { get; set; }

        public Guid CartaConducaoRestricaoId { get; set; }
        public CartaConducaoRestricao? CartaConducaoRestricao { get; set; }

        public Guid CartaConducaoId { get; set; }
        public CartaConducaoEntity? CartaConducao { get; set; }

        public string? Anotacoes { get; set; }
    }
}