using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs
{
    public class LoteDirectTableDTO : IDto 
    {
        public Guid Id { get; set; }
        public Guid? UtenteId { get; set; }
        public string? UtenteNumero { get; set; }
        public string? UtenteNome { get; set; }
        public string? Credencial { get; set; }
        public string? MesAno { get; set; }
        public int? NumeroLote { get; set; }
        public int? CodigoOrganismo { get; set; }

        /// <summary>Abreviatura / sigla do organismo (via <c>CodigoULSNova</c> = código no lote).</summary>
        public string? OrganismoSigla { get; set; }
        public string? OrganismoNome { get; set; }
        public decimal? ValorTaxas { get; set; }
        public decimal? ValorTotal { get; set; }
        public int? TipoServico { get; set; }
        public string? TipoServicoDesignacao { get; set; }
        public int? TipoLote { get; set; }
        public string? TipoLoteDesignacao { get; set; }
        public int? Isencao { get; set; }
        public bool Historico { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}