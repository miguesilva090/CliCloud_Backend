using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService.DTOs;

public class AtualizarConfiguracaoEmailAutomaticaRequest : IDto 
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ativo { get; set; }
    public int Diasantecedencia { get; set; }
    public string Textomensagem { get; set; } = string.Empty;
}