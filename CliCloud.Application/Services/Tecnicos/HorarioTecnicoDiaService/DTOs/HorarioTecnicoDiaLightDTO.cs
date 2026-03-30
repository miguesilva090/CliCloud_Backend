using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs
{
    public class HorarioTecnicoDiaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioTecnicoId { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public Periodo Periodo { get; set; }
        public string? Inicio { get; set; }
        public string? Fim { get; set; }
        public string? Sala { get; set; }
    }
}
