using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AcordosService.DTOs
{
    public class AcordosDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TipoExameId { get; set; }
        public string? TipoExameDesignacao { get; set; }
        public Guid OrganismoId { get; set; }
        public string? OrganismoNome { get; set; }
        public string? CodigoSubsistema { get; set; }
        public decimal? ValTipoExame { get; set; }
        public decimal? ValorOrganismo { get; set; }
        public decimal? MargemOrganismo { get; set; }
        public decimal? ValorUtente { get; set; }
        public bool Inactivo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
