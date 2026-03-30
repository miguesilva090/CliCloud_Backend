using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class HistoricoSmsTabelaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }

        public Guid IdMensagem { get; set; }
        public string TextoMensagem { get; set; } = string.Empty;
        public string NumeroDestinatario { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? MensagemErro { get; set; }

        public DateTime DataHoraCriacao { get; set; }
        public DateTime? DataHoraEnvio { get; set; }

        public string Modulo { get; set; } = string.Empty;
        public int? CodigoUtente { get; set; }
        public string? CodigoMedico { get; set; }
        public int? CodigoFisioterapeuta { get; set; }
        public int? CodigoConsulta { get; set; }
        public int? CodigoTratamento { get; set; }
        public int? CodigoAula { get; set; }
    }
}