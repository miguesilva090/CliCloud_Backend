using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.BancoService.DTOs
{
    public class BancoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NumeroContribuinte { get; set; }
    }
}
