using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class EnviarSmsPorCodigoRequest : IDto
    {
        public string CodigoConfiguracao { get; set; } = string.Empty;

        public string NumeroDestinatario { get; set; } = string.Empty;

        public string NomeUtente { get; set; } = string.Empty;
        public string? NomeMedicoOuProfissional { get; set; }
        public string? NomeEspecialidade { get; set; }
        public string? NumeroSessao { get; set; }

        public DateTime? Data { get; set; }
        public string? Hora { get; set; }

        public string Modulo { get; set; } = "SMSManualCodigo";

        public int? CodigoUtente { get; set; }
        public string? CodigoMedico { get; set; }
        public int? CodigoFisioterapeuta { get; set; }
        public int? CodigoConsulta { get; set; }
        public int? CodigoTratamento { get; set; }
        public int? CodigoAula { get; set; }
    }
}