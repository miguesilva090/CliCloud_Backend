using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Domain.Entities.Common.Configurations;

[Table("ConfigWebService", Schema = "Core")]
public class ConfigWebService : AuditableEntity
{
    [Required]
    public Guid ClinicaId { get; set; }

    public string? UrlRnu { get; set; }
    public string? UrlAcss { get; set; }
    public string? LoginAcss { get; set; }
    public string? PasswordAcss { get; set; }
    public bool UsarProxy { get; set; }
    public string? UserProxy { get; set; } 
    public string? PasswordProxy { get; set; }
    public string? DominioProxy { get; set; }

    public string? UrlAcssRsp { get; set; }
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

    public int VersaoPrescricao { get; set; } = 2;
}