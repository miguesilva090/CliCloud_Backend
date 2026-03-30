using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs
{
    public class HorarioMedicoDiaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioMedicoId { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public Periodo Periodo { get; set; }
        public TimeSpan? Inicio { get; set; }
        public TimeSpan? Fim { get; set; }
        public string? Sala { get; set; }
    }
}
