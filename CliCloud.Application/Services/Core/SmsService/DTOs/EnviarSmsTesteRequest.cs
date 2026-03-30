using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class EnviarSmsTesteRequest : IDto
    {
        public string NumeroDestinatario { get; set; } = string.Empty;
        public string TextoMensagem { get; set; } = string.Empty;
        public string Modulo { get; set; } = "TesteSMS";

        public int? CodigoUtente { get; set; }
        public string? CodigoMedico { get; set; }
        public int? CodigoFisioterapeuta { get; set; }
        public int? CodigoConsulta { get; set; }
        public int? CodigoTratamento { get; set; }
        public int? CodigoAula { get; set; }
    }
}
