using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Prescricao.SpmsPrescricaoSoapService.DTOs;

public class RegistoPrescricaoRspRequest : IDto
{
    public string CodigoOperacao { get; set; } = "REG";
    public DateTime? EnviadoEmUtc { get; set; }
    public DateTime? AtivadoEmUtc { get; set; }
    public string? ChavePedido { get; set; }
    public string? ChavePedidoRelacionado { get; set; }
    public string CorpoXml { get; set; } = string.Empty;
}
