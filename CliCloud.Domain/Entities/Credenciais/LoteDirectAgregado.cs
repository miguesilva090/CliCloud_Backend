#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Credenciais 
{
    [Table("LoteDirectAgregado", Schema = "Credenciais")]
    public class LoteDirectAgregado : AuditableEntityWithSoftDelete
    {
        public int Indice { get; set; }
        public int NumeroLote { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int CodigoOrganismo { get; set; }
        public int TipoLote { get; set; }
        public int TipoServico { get; set; }

        public DateTime DataLote { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorTaxa { get; set; }
        public int? Isencao { get; set; }
        public int NumeroRequisicoes { get; set; }
    }
}