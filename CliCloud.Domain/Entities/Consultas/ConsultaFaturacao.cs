#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;


namespace CliCloud.Domain.Entities.Consultas
{
    [Table("ConsultaFaturacao", Schema = "Consultas")]
    public class ConsultaFaturacao : AuditableEntityWithSoftDelete
    {
        public Guid? ConsultaId { get; set; }
        public Consulta? Consulta { get; set; }

        public int? Isencao { get; set; }

        public Guid? TipoDocumentoId { get; set; }
        public TipoDocumento? TipoDocumento { get; set; }

        public Guid? DocumentoId { get; set; }
        public Documento? Documento { get; set; }

        public decimal? Desconto { get; set; }

        public int? TipoCambio { get; set; }

        public bool Pago { get; set; }

        public bool Faturado { get; set; }

        public string? NumBeneficiario { get; set; }

        public string? Apolice { get; set; }
    }
}