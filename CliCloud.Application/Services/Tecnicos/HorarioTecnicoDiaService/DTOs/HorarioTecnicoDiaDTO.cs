using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoDiaService.DTOs
{
    public class HorarioTecnicoDiaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid HorarioTecnicoId { get; set; }
        public HorarioTecnicoLightDTO? HorarioTecnico { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public Periodo Periodo { get; set; }
        public string? Inicio { get; set; }
        public string? Fim { get; set; }
        public string? Sala { get; set; }
        public int? NumMarcacoesPeriodo { get; set; }
        public int? NumMarcacoesOutro { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
