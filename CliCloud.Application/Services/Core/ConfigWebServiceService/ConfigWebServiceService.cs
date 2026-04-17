using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;
using CliCloud.Application.Services.Core.ConfigWebServiceService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService;

public class ConfigWebServiceService(IRepositoryAsync repository) : IConfigWebServiceService
{
    private readonly IRepositoryAsync _repository = repository;
    private const string SecretMask = "********";

    public async Task<Response<ConfigWebServiceDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        var spec = new ConfigWebServicePorClinicaSpec(clinicaId);

        var entity = (await _repository.GetListAsync<ConfigWebService, Guid>(spec)).FirstOrDefault();

        if (entity is null)
            return ResponseFactory.Success(new ConfigWebServiceDTO{
                Id = Guid.Empty,
                ClinicaId = clinicaId,
                UsarProxy = false,
                UsarProxyRsp = false,
                VersaoPrescricao = 2,
            });

        return ResponseFactory.Success(new ConfigWebServiceDTO{
            Id = entity.Id,
            ClinicaId = entity.ClinicaId,
            UrlRnu = entity.UrlRnu,
            UrlAcss = entity.UrlAcss,
            LoginAcss = entity.LoginAcss,
            PasswordAcss = MaskSecret(entity.PasswordAcss),
            UsarProxy = entity.UsarProxy,
            UserProxy = entity.UserProxy,
            PasswordProxy = MaskSecret(entity.PasswordProxy),
            DominioProxy = entity.DominioProxy,
            UrlAcssRsp = entity.UrlAcssRsp,
            LoginAcssRsp = entity.LoginAcssRsp,
            PasswordAcssRsp = MaskSecret(entity.PasswordAcssRsp),
            UsarProxyRsp = entity.UsarProxyRsp,
            UserProxyRsp = entity.UserProxyRsp,
            PasswordProxyRsp = MaskSecret(entity.PasswordProxyRsp),
            DominioProxyRsp = entity.DominioProxyRsp,
            ProxyAutenticacao = entity.ProxyAutenticacao,
            TokenAutenticacao = MaskSecret(entity.TokenAutenticacao),
            LoginAutenticacao = entity.LoginAutenticacao,
            PasswordAutenticacao = MaskSecret(entity.PasswordAutenticacao),
            VersaoPrescricao = entity.VersaoPrescricao,
        });
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigWebServiceRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.UrlRnu)) return ResponseFactory.Fail<Guid>("URL RNU é obrigatório");
        if(string.IsNullOrWhiteSpace(request.UrlAcss)) return ResponseFactory.Fail<Guid>("URL ACSS é obrigatório");
        if(string.IsNullOrWhiteSpace(request.LoginAcss)) return ResponseFactory.Fail<Guid>("Login ACSS é obrigatório");
        if(string.IsNullOrWhiteSpace(request.PasswordAcss)) return ResponseFactory.Fail<Guid>("Password ACSS é obrigatória");
        if(string.IsNullOrWhiteSpace(request.UrlAcssRsp)) return ResponseFactory.Fail<Guid>("URL ACSS RSP é obrigatório");
        if(string.IsNullOrWhiteSpace(request.LoginAcssRsp)) return ResponseFactory.Fail<Guid>("Login ACSS RSP é obrigatório");
        if(string.IsNullOrWhiteSpace(request.PasswordAcssRsp)) return ResponseFactory.Fail<Guid>("Password ACSS RSP é obrigatória");
        if(request.UsarProxy)
        {
            if (string.IsNullOrWhiteSpace(request.DominioProxy))
                return ResponseFactory.Fail<Guid>("Domínio do proxy é obrigatório quando o proxy está ativo.");
            if (string.IsNullOrWhiteSpace(request.UserProxy))
                return ResponseFactory.Fail<Guid>("Utilizador do proxy é obrigatório quando o proxy está ativo.");
            if (string.IsNullOrWhiteSpace(request.PasswordProxy))
                return ResponseFactory.Fail<Guid>("Password do proxy é obrigatória quando o proxy está ativo.");
        }
        if(request.UsarProxyRsp)
        {
            if (string.IsNullOrWhiteSpace(request.DominioProxyRsp))
                return ResponseFactory.Fail<Guid>("Domínio do proxy RSP é obrigatório quando o proxy está ativo.");
            if (string.IsNullOrWhiteSpace(request.UserProxyRsp))
                return ResponseFactory.Fail<Guid>("Utilizador do proxy RSP é obrigatório quando o proxy está ativo.");
            if (string.IsNullOrWhiteSpace(request.PasswordProxyRsp))
                return ResponseFactory.Fail<Guid>("Password do proxy RSP é obrigatória quando o proxy está ativo.");
        }
        if(request.VersaoPrescricao <= 0)
            return ResponseFactory.Fail<Guid>("Versão da prescrição inválida.");

        var spec = new ConfigWebServicePorClinicaSpec(clinicaId);
        var entity = (await _repository.GetListAsync<ConfigWebService, Guid>(spec)).FirstOrDefault()
            ?? new ConfigWebService{ ClinicaId = clinicaId};

        entity.UrlRnu = request.UrlRnu?.Trim();
        entity.UrlAcss = request.UrlAcss?.Trim();
        entity.LoginAcss = request.LoginAcss?.Trim();
        entity.PasswordAcss = ResolveSecret(request.PasswordAcss, entity.PasswordAcss);
        entity.UsarProxy = request.UsarProxy;
        entity.UserProxy = request.UserProxy?.Trim();
        entity.PasswordProxy = ResolveSecret(request.PasswordProxy, entity.PasswordProxy);
        entity.DominioProxy = request.DominioProxy?.Trim();

        entity.UrlAcssRsp = request.UrlAcssRsp?.Trim();
        entity.LoginAcssRsp = request.LoginAcssRsp?.Trim();
        entity.PasswordAcssRsp = ResolveSecret(request.PasswordAcssRsp, entity.PasswordAcssRsp);
        entity.UsarProxyRsp = request.UsarProxyRsp;
        entity.UserProxyRsp = request.UserProxyRsp?.Trim();
        entity.PasswordProxyRsp = ResolveSecret(request.PasswordProxyRsp, entity.PasswordProxyRsp);
        entity.DominioProxyRsp = request.DominioProxyRsp?.Trim();

        entity.ProxyAutenticacao = request.ProxyAutenticacao?.Trim();
        entity.TokenAutenticacao = ResolveSecret(request.TokenAutenticacao, entity.TokenAutenticacao);
        entity.LoginAutenticacao = request.LoginAutenticacao?.Trim();
        entity.PasswordAutenticacao = ResolveSecret(request.PasswordAutenticacao, entity.PasswordAutenticacao);
        entity.VersaoPrescricao = request.VersaoPrescricao;

        if ( entity.Id == Guid.Empty)
            await _repository.CreateAsync<ConfigWebService, Guid>(entity);
        else 
            await _repository.UpdateAsync<ConfigWebService, Guid>(entity);

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);

    }

    public async Task<Response<int>> ObterVersaoPrescricaoAsync(Guid clinicaId)
    {
        var spec = new ConfigWebServicePorClinicaSpec(clinicaId);
        var entity = (await _repository.GetListAsync<ConfigWebService, Guid>(spec)).FirstOrDefault();
        return ResponseFactory.Success(entity?.VersaoPrescricao ?? 2);
    }

    private static string? MaskSecret(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? value : SecretMask;
    }

    private static string? ResolveSecret(string? incoming, string? existing)
    {
        if (string.IsNullOrWhiteSpace(incoming))
            return existing;

        var trimmed = incoming.Trim();
        if (trimmed == SecretMask)
            return existing;

        return incoming;
    }
}