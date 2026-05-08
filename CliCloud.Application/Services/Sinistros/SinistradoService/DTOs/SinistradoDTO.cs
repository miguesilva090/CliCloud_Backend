using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class SinistradoDTO : IDto 
    {
        public Guid Id { get; set; }
        public string CodigoSinistro { get; set; } = string.Empty;
        public Guid UtenteId { get; set; }
        public Guid? EstadoSinistroId { get; set; }
        public string? EstadoSinistroDesignacao { get; set; }
        public DateTime? DataAcidente { get; set; }
        public bool Historico { get; set; }
        public string? NumeroProcesso { get; set; }
        public string? Observacoes { get; set; }
        public string? Relatorio { get; set; }
        public List<SinistradoLinhaServicoDTO> LinhasServico { get; set; } = [];
    }
}