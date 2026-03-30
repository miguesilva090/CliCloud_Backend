using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Tecnicos.TecnicoService.DTOs;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs
{
    public class HorarioTecnicoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TecnicoId { get; set; }
        public TecnicoLightDTO? Tecnico { get; set; }
        public int? TipoHorario { get; set; }
        public string? MinMarcacao { get; set; }
        public int? HoraComp { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        // TODO: Adicionar Horarios quando HorarioTecnicoDiaService for criado
        // public IEnumerable<HorarioTecnicoDiaLightDTO>? Horarios { get; set; }
    }
}
