using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs
{
    public class TipoLoteLightDTO : IDto
    {
        public int Codigo { get; set; }
        public string? Designa { get; set; }
    }
}
