using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs
{
    public class UpdateHorarioTecnicoVariavelRequest : IDto
    {
        public required string TecnicoId { get; set; }
        public required DateTime Data { get; set; }
        public string? ManhaInicio { get; set; }
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
    }
}