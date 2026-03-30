using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Antecedentes.AntecedentesCirurgicosService.DTOs
{
    public class AntecedentesCirurgicosDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public int? Ano { get; set; }
        public string? TipoCirurgia { get; set; }
        public bool? HouveComplicacoes { get; set; }
        public string? Complicacoes { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}

