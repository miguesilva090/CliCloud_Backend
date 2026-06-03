using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.DTOs
{
    public class SinistradoLinhaServicoDTO : IDto
    {
        public Guid? Id { get; set; }
        public Guid? TratamentoId { get; set; }
        public Guid? AdmissaoId { get; set; }
        public Guid? ServicoId { get; set; }
        public string CodigoServico { get; set; } = string.Empty;
        public string? DesignacaoServico { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorContratado { get; set; }
        public DateTime? DataServico { get; set; }
        public int? NumeroFaturaInterno { get; set; }
        public string? NumeroTFatura { get; set; }
        public DateTime? DataFatura { get; set; }

    }
}