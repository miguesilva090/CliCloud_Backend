using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;

public class AtualizarConfigWebServiceRequest : IDto 
{
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

public class AtualizarConfigWebServiceRequestValidator : AbstractValidator<AtualizarConfigWebServiceRequest>
{
    public AtualizarConfigWebServiceRequestValidator()
    {
        RuleFor(x => x.UrlRnu).NotEmpty().WithMessage("URL RNU é obrigatória.");
        RuleFor(x => x.UrlAcss).NotEmpty().WithMessage("URL ACSS é obrigatória.");
        RuleFor(x => x.LoginAcss).NotEmpty().WithMessage("Login ACSS é obrigatório.");
        RuleFor(x => x.PasswordAcss).NotEmpty().WithMessage("Password ACSS é obrigatória.");
        RuleFor(x => x.UrlAcssRsp).NotEmpty().WithMessage("URL ACSS RSP é obrigatória.");
        RuleFor(x => x.LoginAcssRsp).NotEmpty().WithMessage("Login ACSS RSP é obrigatório.");
        RuleFor(x => x.PasswordAcssRsp).NotEmpty().WithMessage("Password ACSS RSP é obrigatória.");

        RuleFor(x => x.DominioProxy)
            .NotEmpty()
            .When(x => x.UsarProxy)
            .WithMessage("Domínio do proxy é obrigatório quando o proxy está ativo.");
        RuleFor(x => x.UserProxy)
            .NotEmpty()
            .When(x => x.UsarProxy)
            .WithMessage("Utilizador do proxy é obrigatório quando o proxy está ativo.");
        RuleFor(x => x.PasswordProxy)
            .NotEmpty()
            .When(x => x.UsarProxy)
            .WithMessage("Password do proxy é obrigatória quando o proxy está ativo.");

        RuleFor(x => x.DominioProxyRsp)
            .NotEmpty()
            .When(x => x.UsarProxyRsp)
            .WithMessage("Domínio do proxy RSP é obrigatório quando o proxy está ativo.");
        RuleFor(x => x.UserProxyRsp)
            .NotEmpty()
            .When(x => x.UsarProxyRsp)
            .WithMessage("Utilizador do proxy RSP é obrigatório quando o proxy está ativo.");
        RuleFor(x => x.PasswordProxyRsp)
            .NotEmpty()
            .When(x => x.UsarProxyRsp)
            .WithMessage("Password do proxy RSP é obrigatória quando o proxy está ativo.");

        RuleFor(x => x.VersaoPrescricao)
            .InclusiveBetween(1, 2)
            .WithMessage("Versão da prescrição inválida.");
    }
}