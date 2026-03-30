using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs
{
    public class HorarioMedicoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public int? TipoHorario { get; set; }
        public bool HoraComp { get; set; }
        public bool HorarioFlexivel { get; set; }
    }
}
