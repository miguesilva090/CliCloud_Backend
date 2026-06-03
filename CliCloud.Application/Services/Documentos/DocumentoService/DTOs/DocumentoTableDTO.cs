using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
     public class DocumentoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public string? TipoDocumentoAbreviatura { get; set; }
        public string? TipoSerie { get; set; }
        public int AnoFiscal { get; set; }
        public int NumeroDocumento { get; set; }
        public string? NumeroExibicao { get; set; }
        public DateTime? Data { get; set; }
        public Guid? UtenteId { get; set; }
        public string? UtenteNome { get; set; }
        public Guid? OrganismoId { get; set; }
        public string? OrganismoNome { get; set; }
        public Guid? FuncionarioId { get; set; }
        public string? FuncionarioNome { get; set; }
        public decimal? TotalDocumento { get; set; }
        public decimal? TotalIva { get; set; }
        public decimal? TotalDesconto { get; set; }
        public decimal? TotalLiquido { get; set; }
        public CondicaoPagamento? CondicaoPagamento { get; set; }
        public TipoModoPagamento? TipoModoPagamento { get; set; }
        public int? Estado { get; set; }
        public int? EstadoDocumento { get; set; }
        public string? EstadoDocumentoLabel { get; set; }
        public ModuloOrigemDocumento? ModuloOrigem { get; set; }
        public string? OrigemLabel { get; set; }
        public bool Liquidado { get; set; }
        public bool Rectificado { get; set; }
        public bool Exportado { get; set; }
        public bool Anulado { get; set; }
        public bool EstaEmitido { get; set; }
        public string? NomeCliente { get; set; }
        public string? NumeroContribuinteCliente { get; set; }
        /// <summary>Documento de origem (ex.: fatura associada a NC) — legado coluna Ref.</summary>
        public string? ReferenciaDocumento { get; set; }
        /// <summary>Resumo de admissões (C-/T-) — legado coluna Admissões.</summary>
        public string? AdmissoesResumo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
