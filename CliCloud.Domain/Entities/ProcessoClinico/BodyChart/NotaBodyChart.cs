#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Domain.Entities.ProcessoClinico.BodyChart
{
    [Table("NotaBodyChart", Schema = "ProcessoClinico")]
    public class NotaBodyChart : AuditableEntity
    {
        public Guid TratamentoId {get;set;}
        public Tratamento Tratamento {get;set;} = null!;

        public Guid MapaBodyChartId {get;set;}
        public MapaBodyChart MapaBodyChart {get;set;} = null!;

        public Guid MarcadorBodyChartId {get;set;}
        public MarcadorBodyChart MarcadorBodyChart {get;set;} = null!;

        public string? Titulo {get;set;}
        public string? Descricao {get;set;}

        public decimal XPercent {get;set;}
        public decimal YPercent {get;set;}
    }
}