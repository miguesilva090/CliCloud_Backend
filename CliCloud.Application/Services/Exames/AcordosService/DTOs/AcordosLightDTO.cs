using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AcordosService.DTOs
{
    public class AcordosLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string? CodigoSubsistema { get; set; }
        public string? TipoExameDesignacao { get; set; }
        public string? OrganismoNome { get; set; }
    }
}
