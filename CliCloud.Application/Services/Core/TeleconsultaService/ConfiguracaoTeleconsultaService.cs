using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.TeleconsultaService.DTOs;
using CliCloud.Application.Services.Core.TeleconsultaService.Specifications;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.TeleconsultaService
{
  public class ConfiguracaoTeleconsultaService(IRepositoryAsync repository) : IConfiguracaoTeleconsultaService
  {
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfiguracaoTeleconsultaDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
      try
      {
        var spec = new ConfiguracaoTeleconsultaPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoTeleconsulta, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
          return ResponseFactory.Fail<ConfiguracaoTeleconsultaDTO>("Configuração de teleconsulta não encontrada.");

        return ResponseFactory.Success(MapToDTO(entidade));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoTeleconsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoTeleconsultaRequest request)
    {
      try
      {
        var spec = new ConfiguracaoTeleconsultaPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoTeleconsulta, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
        {
          entidade = new ConfiguracaoTeleconsulta { ClinicaId = clinicaId };
          AplicarConfiguracao(entidade, request);
          var criado = await _repository.CreateAsync<ConfiguracaoTeleconsulta, Guid>(entidade);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(criado.Id);
        }

        AplicarConfiguracao(entidade, request);
        var atualizado = await _repository.UpdateAsync<ConfiguracaoTeleconsulta, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(atualizado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void AplicarConfiguracao(
      ConfiguracaoTeleconsulta entidade,
      AtualizarConfiguracaoTeleconsultaRequest request
    )
    {
      entidade.Ativo = request.Ativo;
      entidade.Provider = "jitsi";
      entidade.BaseMeetingUrl = string.IsNullOrWhiteSpace(request.BaseMeetingUrl)
        ? "https://meet.jit.si"
        : request.BaseMeetingUrl.Trim();
      entidade.JwtAtivo = request.JwtAtivo;
      entidade.JwtAppId = string.IsNullOrWhiteSpace(request.JwtAppId) ? null : request.JwtAppId.Trim();
      entidade.JwtApiKey = string.IsNullOrWhiteSpace(request.JwtApiKey) ? null : request.JwtApiKey.Trim();
      entidade.JwtKid = string.IsNullOrWhiteSpace(request.JwtKid) ? null : request.JwtKid.Trim();
      entidade.JwtPrivateKey = string.IsNullOrWhiteSpace(request.JwtPrivateKey) ? null : request.JwtPrivateKey.Trim();
      entidade.JanelaEntradaMinutosAntes = request.JanelaEntradaMinutosAntes < 0 ? 0 : request.JanelaEntradaMinutosAntes;
      entidade.DuracaoPadraoMinutos = request.DuracaoPadraoMinutos < 5 ? 30 : request.DuracaoPadraoMinutos;
      entidade.PermitirEntradaAntesDoInicio = request.PermitirEntradaAntesDoInicio;
      entidade.LobbyAtivo = request.LobbyAtivo;
    }

    private static ConfiguracaoTeleconsultaDTO MapToDTO(ConfiguracaoTeleconsulta entidade)
    {
      return new ConfiguracaoTeleconsultaDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        Ativo = entidade.Ativo,
        Provider = entidade.Provider,
        BaseMeetingUrl = entidade.BaseMeetingUrl,
        JwtAtivo = entidade.JwtAtivo,
        JwtAppId = entidade.JwtAppId,
        JwtApiKey = entidade.JwtApiKey,
        JwtKid = entidade.JwtKid,
        JwtPrivateKey = entidade.JwtPrivateKey,
        JanelaEntradaMinutosAntes = entidade.JanelaEntradaMinutosAntes,
        DuracaoPadraoMinutos = entidade.DuracaoPadraoMinutos,
        PermitirEntradaAntesDoInicio = entidade.PermitirEntradaAntesDoInicio,
        LobbyAtivo = entidade.LobbyAtivo,
      };
    }
  }
}
