using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs
{
    public class HorarioTecnicoDiaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioTecnicoId { get; set; }
        public string? HorarioTecnicoTecnicoNome { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public string DiaSemanaNome { get; set; } = string.Empty;
        public Periodo Periodo { get; set; }
        public string PeriodoNome { get; set; } = string.Empty;
        public string? Inicio { get; set; }
        public string? Fim { get; set; }
        public string? Sala { get; set; }
        public int? NumMarcacoesPeriodo { get; set; }
        public int? NumMarcacoesOutro { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
