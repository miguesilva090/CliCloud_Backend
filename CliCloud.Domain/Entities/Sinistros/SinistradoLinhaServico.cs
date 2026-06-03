#nullable enable 

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Sinistros
{
    [Table("SinistradoLinhaServico", Schema = "Sinistros")]
    public class SinistradoLinhaServico : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public Guid SinistradoId { get; set; }

        /// <summary>Serviço da clínica (equivalente legado LINHASINISTRO.C_Servico resolvido).</summary>
        public Guid? ServicoId { get; set; }

        [Required]
        /// <summary>Texto auxiliar: GUID, CONS-{consulta:N} ou TRAT-{tratamento:N} quando sem ServicoId.</summary>
        [StringLength(40)]
        public string CodigoServico { get; set; } = string.Empty;

        [StringLength(160)]
        public string? DesignacaoServico { get; set; }
        public int Quantidade { get; set; } = 1;
        public decimal? ValorServico { get; set; }
        public decimal? ValorContratado { get; set; }
        public DateTime? DataServico { get; set; }
        public int? NumeroFaturaInterno { get; set; }
        public string? NumeroTFatura { get; set; }
        public DateTime? DataFatura { get; set; }
        public Guid? TratamentoId { get; set; }
        public Guid? AdmissaoId { get; set; }
        public Sinistrado Sinistrado { get; set; } = null!;
    }
}