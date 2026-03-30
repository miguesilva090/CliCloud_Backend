using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs
{
    public class HorarioMedicoDiaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioMedicoId { get; set; }
        public HorarioMedicoLightDTO? HorarioMedico { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public Periodo Periodo { get; set; }
        public TimeSpan? Inicio { get; set; }
        public TimeSpan? Fim { get; set; }
        public string? Sala { get; set; }
        public int? Vagas { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
