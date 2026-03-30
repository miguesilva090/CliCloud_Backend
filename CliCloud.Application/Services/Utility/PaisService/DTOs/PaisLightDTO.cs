using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.PaisService.DTOs
{
    public class PaisLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
        public string? Prefixo { get; set; }
       
    }
}

