using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ConfigCartaConducaoService.DTOs
{
    public class AtualizarConfigCartaConducaoRequest : IDto 
    {
        public string? UrlOnline { get; set; }
        public string? UrlOffline { get; set; }
        public string? Utilizador { get; set; }
        public string? Password { get; set; }
        public int AutoridadeSaudePublica { get; set; }
    }
}