#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.ProcessoClinico.BodyChart
{
    [Table("MarcadorBodyChart", Schema = "ProcessoClinico")]
    public class MarcadorBodyChart : AuditableEntityWithSoftDelete
    {
        public Guid MapaBodyChartId { get; set; }
        public MapaBodyChart MapaBodyChart { get; set; } = null!;

        public string Titulo { get; set; } = string.Empty;
        public string CorHex { get; set; } = string.Empty;

        public ICollection<NotaBodyChart> Notas {get;set;} = [];
    }
}