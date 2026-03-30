using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProvenienciasUtente.ProvenienciaUtenteService.DTOs
{
    public class ProvenienciaUtenteTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
