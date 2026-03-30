using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs
{
    public class MargemMedicoTableDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid ServicoId { get; set; }
        public string? ServicoDesignacao { get; set; }
        public Guid MedicoId { get; set; }
        public string? MedicoNome { get; set; }
        public string? MedicoNumeroContribuinte { get; set; }
        public decimal? ValorMargem { get; set; }
        public decimal? PercentagemMargem { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
