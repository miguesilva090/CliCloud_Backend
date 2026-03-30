using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.TensaoArterialService.DTOs
{
    public class TensaoArterialDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public int TensaoSistolica { get; set; }
        public int TensaoDiastolica { get; set; }
        public string? Observacoes { get; set; }
    }
}

