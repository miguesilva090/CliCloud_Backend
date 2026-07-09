#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Domain.Entities.Credenciais 
{
    [Table("LoteDirectLinha789", Schema = "Credenciais")]
    public class LoteDirectLinha789 : AuditableEntityWithSoftDelete
    {
        public Guid LoteDirectId { get; set; }
        public LoteDirect LoteDirect { get; set; } = null!;
        public Guid ServicoId { get; set; }
        public Servico Servico { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorUtenteOriginal { get; set; }
        public decimal ValorInstituicaoOriginal { get; set; }
        public decimal ValorUtente { get; set; }
        public decimal ValorInstituicao { get; set; }
        
    }
}