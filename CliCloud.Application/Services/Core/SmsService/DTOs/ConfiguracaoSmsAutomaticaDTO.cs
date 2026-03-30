using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class ConfiguracaoSmsAutomaticaDTO : IDto 
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public int Ativo { get; set; }
        public int Diasantecedencia { get; set; }
        public string Textomensagem { get; set; } = string.Empty;
        public bool TodosMedicos { get; set; }

    }
}