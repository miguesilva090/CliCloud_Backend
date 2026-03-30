using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Medicos.MedicoService.DTOs;
using CliCloud.Application.Services.Servicos.ServicoService.DTOs;

namespace CliCloud.Application.Services.Medicos.MargemMedicoService.DTOs
{
    public class MargemMedicoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid ServicoId { get; set; }
        public string? ServicoDesignacao { get; set; }
        public Guid MedicoId { get; set; }
        public MedicoLightDTO? Medico { get; set; }
        public decimal? ValorMargem { get; set; }
        public decimal? PercentagemMargem { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
