using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService.DTOs
{
    public class AtualizarConfiguracaoSmsRequest : IDto 
    {
        public bool Ativo { get; set; }
        public int UsenditArpoone { get; set; } = 1;

        public string? Url { get; set; }
        public string? Loginapi { get; set; }
        public string? Passwordapi { get; set; } 
        public string? Numapi { get; set; }
        public string? Remetente { get; set; }

        public string? ArpooneUrl { get; set; }
        public string? ArpooneSender { get; set; }
        public string? ArpooneApiKey { get; set; }
        public Guid? ArpooneOrganizationID { get; set; }
    }
}