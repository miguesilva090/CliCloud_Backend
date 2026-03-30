using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Organismos.OrganismoService.DTOs
{
    public class OrganismoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeComercial { get; set; }
        public string? Abreviatura { get; set; }
        public string? NumeroContribuinte { get; set; }
    }
}
