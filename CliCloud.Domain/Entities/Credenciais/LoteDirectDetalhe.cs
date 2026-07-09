#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Credenciais
{
    [Table("LoteDirectDetalhe", Schema = "Credenciais")]
    public class LoteDirectDetalhe : AuditableEntityWithSoftDelete
    {
        public Guid LoteDirectAgregadoId { get; set; }
        public LoteDirectAgregado LoteDirectAgregado { get; set; } = null!;
        public Guid LoteDirectId { get; set; }
        public LoteDirect LoteDirect { get; set; } = null!;
        public int Indice { get; set; }
        public int NumeroLote { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int CodigoOrganismo { get; set; }
        public int TipoServico { get; set; }
        public int TipoLote { get; set; }
        public string? Credencial { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorTaxa { get; set; }
        public int? Isencao { get; set; }
        public DateTime? Data { get; set; }
    }
}
