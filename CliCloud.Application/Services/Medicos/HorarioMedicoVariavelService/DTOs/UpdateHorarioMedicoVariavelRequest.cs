using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.HorarioMedicoVariavelService.DTOs
{
    public class UpdateHorarioMedicoVariavelRequest : IDto
    {
        public required string MedicoId { get; set; }
        public required DateTime Data { get; set; }
        public string? ManhaInicio { get; set; }
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
    }
}
