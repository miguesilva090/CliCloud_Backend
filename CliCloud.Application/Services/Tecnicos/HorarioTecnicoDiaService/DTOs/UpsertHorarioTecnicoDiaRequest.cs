using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs
{
    public class UpsertHorarioTecnicoDiaRequest : IDto
    {
        public string? Id { get; set; } // Se fornecido, atualiza; se não, cria novo
        public required string HorarioTecnicoId { get; set; }
        public required DiaSemana DiaSemana { get; set; }
        public required Periodo Periodo { get; set; }
        public string? Inicio { get; set; }
        public string? Fim { get; set; }
        public string? Sala { get; set; }
        public int? NumMarcacoesPeriodo { get; set; }
        public int? NumMarcacoesOutro { get; set; }
    }
}
