using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs
{
    public class UpsertHorarioMedicoDiaRequest : IDto
    {
        public string? Id { get; set; } // Se fornecido, atualiza; se não, cria novo
        public required string HorarioMedicoId { get; set; }
        public required DiaSemana DiaSemana { get; set; }
        public required Periodo Periodo { get; set; }
        public string? Inicio { get; set; } // TimeSpan como string "HH:mm:ss"
        public string? Fim { get; set; } // TimeSpan como string "HH:mm:ss"
        public string? Sala { get; set; }
        public int? Vagas { get; set; }
    }
}
