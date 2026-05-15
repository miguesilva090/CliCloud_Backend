using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs
{
    public class LoteDirectDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid? UtenteId { get; set; }
        public string? UtenteNome { get; set; }
        public string? Credencial { get; set; }
        public int? NumeroLote { get; set; }
        public int? IndiceLote { get; set; }
        public int? CodigoOrganismo { get; set; }

        /// <summary>Abreviatura do organismo (código ULS = <see cref="CodigoOrganismo"/>).</summary>
        public string? OrganismoSigla { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? TipoServico { get; set; }
        public Guid? TipoServicoRegistoId { get; set; }
        public string? TipoServicoRegistoDescricao { get; set; }
        public int? TipoLote { get; set; }
        public decimal? ValorTaxas { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorTotalV2 { get; set; }
        public decimal? ValorTotalV3 { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? ValorTaxasLinhas { get; set; }
        public bool Historico { get; set; }

        public string? CentroSaude { get; set; }
        public int? Isencao { get; set; }
        public string? Proveniencia { get; set; }
        public bool CredencialExterna { get; set; }
        public string? Servicos { get; set; }

        public Guid? MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? CodigoMedico { get; set; }
        public string? Especialidade { get; set; }
        public Guid? MedicoExternoId { get; set; }
        public string? MedicoExternoNome { get; set; }

        public Guid? ServicoConsultaId { get; set; }
        public string? ServicoConsultaDesignacao { get; set; }

        public string? CodigoServicoConsulta { get; set; }
        public string? ServicoConsulta { get; set; }
        public string? CodigoSubsistemaConsulta { get; set; }
        public int? QuantidadeConsulta { get; set; }
        public decimal? ValorConsulta { get; set; }
        public decimal? TaxaConsulta { get; set; }
        public bool ProcedimentosEfetuados { get; set; }
    }
}
