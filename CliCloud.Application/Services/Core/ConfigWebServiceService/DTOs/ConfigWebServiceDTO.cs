using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;

public class ConfigWebServiceDTO : IDto 
{
    public Guid Id { get; set; }
    public Guid ClinicaId { get; set; }

    public string? UrlRnu { get; set; }
    public string? UrlAcss { get; set; }
    public string? LoginAcss { get; set; }
    public string? PasswordAcss { get; set; }
    public bool UsarProxy { get; set; }
    public string? UserProxy { get; set; }
    public string? PasswordProxy { get; set; }
    public string? DominioProxy { get; set; }

    public  string? UrlAcssRsp { get; set; }
    public string? LoginAcssRsp { get; set; }
    public string? PasswordAcssRsp { get; set; }
    public bool UsarProxyRsp { get; set; }
    public string? UserProxyRsp { get; set; }
    public string? PasswordProxyRsp { get; set; }
    public string? DominioProxyRsp { get; set; }

    public string? ProxyAutenticacao { get; set; }
    public string? TokenAutenticacao { get; set; }
    public string? LoginAutenticacao { get; set; }
    public string? PasswordAutenticacao { get; set; }

    public int VersaoPrescricao { get; set; }
}