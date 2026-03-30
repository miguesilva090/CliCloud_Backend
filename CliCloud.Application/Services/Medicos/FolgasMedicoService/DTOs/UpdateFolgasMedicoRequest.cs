using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs
{
    public class UpdateFolgasMedicoRequest : IDto
    {
        public required string MedicoId { get; set; }
        public required DateTime DataDe { get; set; }
        public required DateTime DataAte { get; set; }
        public bool TodoDia { get; set; }
        public bool MesInteiro { get; set; }
        public string? ManhaInicio { get; set; }
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
    }
}
