using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;

public class ProxyTokenResultDTO : IDto 
{
    public string? Codigo { get; set; }

    public string? Descricao { get; set; }

    public string? Token { get; set; }
}