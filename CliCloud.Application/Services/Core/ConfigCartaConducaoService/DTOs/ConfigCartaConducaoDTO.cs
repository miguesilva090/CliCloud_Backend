using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ConfigCartaConducaoService.DTOs
{
    public class ConfigCartaConducaoDTO : IDto 
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string? UrlOnline { get; set; }
        public string? UrlOffline { get; set; }
        public string? Utilizador { get; set; }
        public string? Password { get; set; }
        public int AutoridadeSaudePublica { get; set; }
    }
}