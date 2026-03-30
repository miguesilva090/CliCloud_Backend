using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.DTOs
{
    public class TipoDocumentoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Abreviatura { get; set; } = string.Empty;
        public bool Inactivo { get; set; }
    }
}
