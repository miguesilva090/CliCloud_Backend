using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.RuaService.DTOs
{
    public class RuaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? FreguesiaId { get; set; }
        public string? FreguesiaNome { get; set; }
        public Guid? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }
        public string? CodigoPostalLocalidade { get; set; }
    }
}

