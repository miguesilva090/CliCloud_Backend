using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Documentos.DocumentoService.DTOs
{
    public class DocumentoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? TipoDocumentoAbreviatura { get; set; }
        public int NumeroDocumento { get; set; }
        public DateTime? Data { get; set; }
        public string? NomeCliente { get; set; }
        public decimal? TotalLiquido { get; set; }
        public int? Estado { get; set; }
        public bool Liquidado { get; set; }
    }
}
