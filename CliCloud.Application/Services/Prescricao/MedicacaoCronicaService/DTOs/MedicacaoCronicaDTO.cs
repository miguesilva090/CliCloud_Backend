using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.MedicacaoCronicaService.DTOs
{
    public class MedicacaoCronicaDTO : IDto 
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public string Cnpem { get; set; } = string.Empty;
        public string? EmbId { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public string? Dosagem { get; set; }
        public string? DescricaoEmbalagem { get; set; }
        public string? FormaFarmaceutica { get; set; }
        public string? PrincipioAtivo { get; set; }
        public string? Posologia { get; set; }
        public int TipoLinha { get; set; } 
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

    }
}