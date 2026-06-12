using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.DTOs
{
    public class ContaBancariaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string? IBAN { get; set; }
    }
}