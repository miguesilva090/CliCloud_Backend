using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ConfigWebServiceService.DTOs;
using CliCloud.Application.Services.Core.ConfigWebServiceService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService;

public class ConfigWebServiceService(IRepositoryAsync repository) : IConfigWebServiceService
{
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfigWebServiceDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
        var spec = new ConfigWebServicePorClinicaSpec(clinicaId);

        var entity = (await _repository.GetListAsync<ConfigWebService, Guid>(spec)).FirstOrDefault();

        if (entity is null)
            return ResponseFactory.Fail<ConfigWebServiceDTO>("Configuração de WebService não encontrada");

        return ResponseFactory.Success(new ConfigWebServiceDTO{
            Id = entity.Id,
            ClinicaId = entity.ClinicaId,
            UrlRnu = entity.UrlRnu,
            UrlAcss = entity.UrlAcss,
            LoginAcss = entity.LoginAcss,
            PasswordAcss = entity.PasswordAcss,
            UsarProxy = entity.UsarProxy,
            UserProxy = entity.UserProxy,
            PasswordProxy = entity.PasswordProxy,
            DominioProxy = entity.DominioProxy,
            UrlAcssRsp = entity.UrlAcssRsp,
            LoginAcssRsp = entity.LoginAcssRsp,
            PasswordAcssRsp = entity.PasswordAcssRsp,
            UsarProxyRsp = entity.UsarProxyRsp,
            UserProxyRsp = entity.UserProxyRsp,
            PasswordProxyRsp = entity.PasswordProxyRsp,
            DominioProxyRsp = entity.DominioProxyRsp,
            ProxyAutenticacao = entity.ProxyAutenticacao,
            TokenAutenticacao = entity.TokenAutenticacao,
            LoginAutenticacao = entity.LoginAutenticacao,
            PasswordAutenticacao = entity.PasswordAutenticacao,
            VersaoPrescricao = entity.VersaoPrescricao,
        });
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfigWebServiceRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.UrlRnu)) return ResponseFactory.Fail<Guid>("URL RNU é obrigatório");
        if(string.IsNullOrWhiteSpace(request.UrlAcss)) return ResponseFactory.Fail<Guid>("URl ACSS é orbigatório");
        if(string.IsNullOrWhiteSpace(request.LoginAcss)) return ResponseFactory.Fail<Guid>("Login ACSS é obrigatório");
        if(string.IsNullOrWhiteSpace(request.PasswordAcss)) return ResponseFactory.Fail<Guid>("Password ACSS é obrigatória");
        if(string.IsNullOrWhiteSpace(request.UrlAcssRsp)) return ResponseFactory.Fail<Guid>("URL ACSS RSP é obrigatório");
        if(string.IsNullOrWhiteSpace(request.LoginAcssRsp)) return ResponseFactory.Fail<Guid>("Login ACSS RSP é orbigatório");
        if(string.IsNullOrWhiteSpace(request.PasswordAcssRsp)) return ResponseFactory.Fail<Guid>("Password ACSS RSP é obrigatória");

        var spec = new ConfigWebServicePorClinicaSpec(clinicaId);
        var entity = (await _repository.GetListAsync<ConfigWebService, Guid>(spec)).FirstOrDefault()
            ?? new ConfigWebService{ ClinicaId = clinicaId};

        entity.UrlRnu = request.UrlRnu?.Trim();
        entity.UrlAcss = request.UrlAcss?.Trim();
        entity.LoginAcss = request.LoginAcss?.Trim();
        entity.PasswordAcss = request.PasswordAcss;
        entity.UsarProxy = request.UsarProxy;
        entity.UserProxy = request.UserProxy?.Trim();
        entity.PasswordProxy = request.PasswordProxy;
        entity.DominioProxy = request.DominioProxy?.Trim();

        entity.UrlAcssRsp = request.UrlAcssRsp?.Trim();
        entity.LoginAcssRsp = request.LoginAcssRsp?.Trim();
        entity.PasswordAcssRsp = request.PasswordAcssRsp;
        entity.UsarProxyRsp = request.UsarProxyRsp;
        entity.UserProxyRsp = request.UserProxyRsp?.Trim();
        entity.PasswordProxyRsp = request.PasswordProxyRsp;
        entity.DominioProxyRsp = request.DominioProxyRsp?.Trim();

        entity.ProxyAutenticacao = request.ProxyAutenticacao?.Trim();
        entity.TokenAutenticacao = request.TokenAutenticacao?.Trim();
        entity.LoginAutenticacao = request.LoginAutenticacao?.Trim();
        entity.PasswordAutenticacao = request.PasswordAutenticacao;
        entity.VersaoPrescricao = request.VersaoPrescricao;

        if ( entity.Id == Guid.Empty)
            await _repository.CreateAsync<ConfigWebService, Guid>(entity);
        else 
            await _repository.UpdateAsync<ConfigWebService, Guid>(entity);

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);

    }
}