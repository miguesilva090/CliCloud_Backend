using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs
{
    public class RelatorioAtestadoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
        public DateTime? AssinadoEm { get; set; }
        public DateTime CreatedOn { get; set; }

        // Dados derivados do médico (para impressão/relatório)
        public string? MedicoNome { get; set; }
        public string? MedicoNumeroProfissional { get; set; }
    }
}

