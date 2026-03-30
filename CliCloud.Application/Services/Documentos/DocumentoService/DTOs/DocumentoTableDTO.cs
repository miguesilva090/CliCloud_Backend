using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
    public class DocumentoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public string? TipoDocumentoAbreviatura { get; set; }
        public int NumeroDocumento { get; set; }
        public DateTime? Data { get; set; }
        public Guid? UtenteId { get; set; }
        public string? UtenteNome { get; set; }
        public Guid? OrganismoId { get; set; }
        public string? OrganismoNome { get; set; }
        public Guid? FuncionarioId { get; set; }
        public string? FuncionarioNome { get; set; }
        public decimal? TotalDocumento { get; set; }
        public decimal? TotalIva { get; set; }
        public decimal? TotalLiquido { get; set; }
        public CondicaoPagamento? CondicaoPagamento { get; set; }
        public TipoModoPagamento? TipoModoPagamento { get; set; }
        public int? Estado { get; set; }
        public bool Liquidado { get; set; }
        public bool Rectificado { get; set; }
        public bool Exportado { get; set; }
        public string? NomeCliente { get; set; }
        public string? NumeroContribuinteCliente { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
