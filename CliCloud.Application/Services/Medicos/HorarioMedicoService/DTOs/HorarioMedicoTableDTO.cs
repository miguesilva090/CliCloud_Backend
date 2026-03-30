using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs
{
    public class HorarioMedicoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? MedicoNumeroContribuinte { get; set; }
        public int? TipoHorario { get; set; }
        public TimeSpan? MinMarcacao { get; set; }
        public bool HoraComp { get; set; }
        public TimeSpan? PrimeiraConsulta { get; set; }
        public bool HorarioFlexivel { get; set; }
        public DateTime CreatedOn { get; set; }
        public int HorariosCount { get; set; }
    }
}
