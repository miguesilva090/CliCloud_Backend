#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.BodyChart
{
    [Table("MapaBodyChart", Schema = "ProcessoClinico")]
    public class MapaBodyChart : AuditableEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string CaminhoImagem { get; set; } = string.Empty;

        public ICollection<MarcadorBodyChart> Marcadores {get;set;} = [];
        public ICollection<NotaBodyChart> Notas {get;set;} = [];
    }
}