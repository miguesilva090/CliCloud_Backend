using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.FolgasMedicoService.DTOs
{
    public class FolgasMedicoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public bool TodoDia { get; set; }
        public bool MesInteiro { get; set; }
        public string? ManhaInicio { get; set; }
        public string? ManhaFim { get; set; }
        public string? TardeInicio { get; set; }
        public string? TardeFim { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
