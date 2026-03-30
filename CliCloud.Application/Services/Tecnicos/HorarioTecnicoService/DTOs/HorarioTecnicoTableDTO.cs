using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoService.DTOs
{
    public class HorarioTecnicoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TecnicoId { get; set; }
        public string? TecnicoNome { get; set; }
        public string? TecnicoNumeroContribuinte { get; set; }
        public int? TipoHorario { get; set; }
        public string? MinMarcacao { get; set; }
        public int? HoraComp { get; set; }
        public DateTime CreatedOn { get; set; }
        public int HorariosCount { get; set; }
    }
}
