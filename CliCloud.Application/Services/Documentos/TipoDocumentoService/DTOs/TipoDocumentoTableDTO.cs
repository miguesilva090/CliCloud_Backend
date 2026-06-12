using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs
{
    public class TipoDocumentoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public string? Abreviatura { get; set; }
        public string? Natureza { get; set; }
        public int? TipoMovimento { get; set; }
        public int? CodigoTipoDocumentoSaft { get; set; }
        public string? NumeroSerie { get; set; }
        public int? NumeroDocumento { get; set; }
        public int? NumVias { get; set; }
        public bool Inactivo { get; set; }
        public bool MostraFaturacao { get; set; }
        public bool DescarregarTesouraria { get; set; }
        public bool Habilitado { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CodigoATCUD { get; set; }
        public string? ATCUDEstado { get; set; }
        public string? TipoSerie { get; set; }
    }
}
