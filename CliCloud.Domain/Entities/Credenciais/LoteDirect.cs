#nullable enable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Credenciais
{
    [Table("LoteDirect", Schema = "Credenciais")]
    public class LoteDirect : AuditableEntityWithSoftDelete
    {
        [Key]
        public new Guid Id { get; set; }
        public Guid? UtenteId { get; set; }
        [StringLength(120)]
        public string? Credencial { get; set; }
        public int? NumeroLote { get; set; }
        public int? IndiceLote { get; set; }
        public int? CodigoOrganismo { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? TipoServico { get; set; }
        public int? TipoLote { get; set; }
        public decimal? ValorTaxas { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorTotalV2 { get; set; }
        public decimal? ValorTotalV3 { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? ValorTaxasLinhas { get; set; }
        public bool Historico { get; set; }
        [StringLength(120)]
        public string? CentroSaude { get; set; }
        public int? Isencao { get; set; }
        [StringLength(120)]
        public string? Proveniencia { get; set; }
        public bool CredencialExterna { get; set; }
        [StringLength(250)]
        public string? Servicos { get; set; }
        public Guid? TipoServicoRegistoId { get; set; }
        public Guid? ServicoConsultaId { get; set; }
        public Guid? MedicoId { get; set; }
        [StringLength(20)]
        public string? CodigoMedico { get; set; }
        [StringLength(120)]
        public string? Especialidade { get; set; }
        public Guid? MedicoExternoId { get; set; }
        [StringLength(40)]
        public string? CodigoServicoConsulta { get; set; }
        [StringLength(120)]
        public string? ServicoConsulta { get; set; }
        [StringLength(40)]
        public string? CodigoSubsistemaConsulta { get; set; }
        public int? QuantidadeConsulta { get; set; }
        public decimal? ValorConsulta { get; set; }
        public decimal? TaxaConsulta { get; set; }
        public bool ProcedimentosEfetuados { get; set; }
        public Utente? Utente { get; set; }
        public Medico? Medico { get; set; }
        public MedicoExterno? MedicoExterno { get; set; }
        public TipoServico? TipoServicoRegisto { get; set; }
        public Servico? ServicoConsultaRegisto { get; set; }
        public ICollection<LoteDirectLinha> Linhas { get; set; } = new List<LoteDirectLinha>();
        public ICollection<LoteDirectLinha789> Linhas789 { get; set; } = new List<LoteDirectLinha789>();
        public ICollection<LoteDirectDetalhe> Detalhes { get; set; } = new List<LoteDirectDetalhe>();
    }
}
