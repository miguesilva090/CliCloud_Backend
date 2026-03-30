using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FreguesiaService.DTOs
{
    public class FreguesiaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public Guid? ConcelhoId { get; set; }
        public string? ConcelhoNome { get; set; }
    }
}

