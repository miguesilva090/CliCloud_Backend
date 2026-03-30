using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class AtualizarConfiguracaoAutomaticaRequest : IDto
    {
        public string Codigo { get; set; } = string.Empty;
        public int Ativo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Diasantecedencia { get; set; }
        public string Textomensagem { get; set; } = string.Empty;
    }
}