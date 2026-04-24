#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CartaConducaoEntity = CliCloud.Domain.Entities.CartaConducao.CartaConducao;

namespace CliCloud.Domain.Entities.Atestados
{
    [Table("AtestadoCategoria", Schema = "Atestados")]
    public class AtestadoCategoria : AuditableEntityWithSoftDelete
    {
        public Guid AtestadoId { get; set; }
        public Atestado? Atestado { get; set; }

        public Guid CartaConducaoId { get; set; }
        public CartaConducaoEntity? CartaConducao { get; set; }

       public int Apto { get; set; }
       public int AptoGrupo2 { get; set; }

    }
}