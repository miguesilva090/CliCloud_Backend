using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Medicos.HorarioMedicoDiaService.DTOs;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoService.DTOs
{
    public class HorarioMedicoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public MedicoLightDTO? Medico { get; set; }
        public int? TipoHorario { get; set; }
        public TimeSpan? MinMarcacao { get; set; }
        public bool HoraComp { get; set; }
        public TimeSpan? PrimeiraConsulta { get; set; }
        public bool HorarioFlexivel { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        
        public IEnumerable<HorarioMedicoDiaDTO>? Horarios {get; set;}
    }
}
