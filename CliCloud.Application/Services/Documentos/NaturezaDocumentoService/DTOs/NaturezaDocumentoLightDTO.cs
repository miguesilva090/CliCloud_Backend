using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs
{
    public class NaturezaDocumentoLightDTO : IDto 
    {
        public Guid Id { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        
    }
}