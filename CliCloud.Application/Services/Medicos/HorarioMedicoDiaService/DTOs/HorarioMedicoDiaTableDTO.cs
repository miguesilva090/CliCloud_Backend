using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs
{
    public class HorarioMedicoDiaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioMedicoId { get; set; }
        public string? HorarioMedicoMedicoNome { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public string DiaSemanaNome { get; set; } = string.Empty;
        public Periodo Periodo { get; set; }
        public string PeriodoNome { get; set; } = string.Empty;
        public TimeSpan? Inicio { get; set; }
        public TimeSpan? Fim { get; set; }
        public string? Sala { get; set; }
        public int? Vagas { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
