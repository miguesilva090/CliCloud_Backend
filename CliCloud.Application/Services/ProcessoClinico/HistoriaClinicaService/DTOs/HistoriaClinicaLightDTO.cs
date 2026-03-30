using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.HistoriaClinicaService.DTOs
{
    public class HistoriaClinicaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public string UtenteNome { get; set; } = string.Empty;
        public Guid MedicoId { get; set; }
        public string MedicoNome { get; set; } = string.Empty;
        public Guid? EspecialidadeId { get; set; }
        public string? EspecialidadeNome { get; set; }
        public DateTime Data { get; set; }
        public string? Hora { get; set; }
        public string Obs { get; set; } = string.Empty;
    }
}

