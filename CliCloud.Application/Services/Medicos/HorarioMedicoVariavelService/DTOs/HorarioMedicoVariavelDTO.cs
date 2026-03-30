using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs
{
    public class HorarioMedicoVariavelDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime Data { get; set; }
        public string? ManhaInicio { get; set; } // TimeSpan como string "HH:mm:ss"
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
