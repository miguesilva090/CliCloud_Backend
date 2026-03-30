using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.AlergiaUtenteService.DTOs
{
    public class AlergiaUtenteDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid? AlergiaId { get; set; }
        public string? AlergiaDescricao { get; set; }
        public Guid? GrauAlergiaId { get; set; }
        public DateOnly? DataDesde { get; set; }
        public DateOnly? DataAte { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
