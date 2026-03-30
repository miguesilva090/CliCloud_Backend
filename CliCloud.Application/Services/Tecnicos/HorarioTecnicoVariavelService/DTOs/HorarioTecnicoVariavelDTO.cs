using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tecnicos.HorarioTecnicoVariavelService.DTOs
{
    public class HorarioTecnicoVariavelDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid TecnicoId { get; set; }
        public DateTime Data { get; set; }
        public string? ManhaInicio { get; set; }
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

