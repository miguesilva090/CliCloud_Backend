using System;
using System.Linq;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.VozService.DTOs;
using CliCloud.Application.Services.Core.VozService.Specifications;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.VozService
{
  public class ServicoVoz(IRepositoryAsync repository) : IServicoVoz
  {
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfiguracaoVozDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
      try
      {
        var spec = new ConfiguracaoVozPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoVoz, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
          return ResponseFactory.Fail<ConfiguracaoVozDTO>("Configuração de voz não encontrada.");

        return ResponseFactory.Success(MapToDTO(entidade));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoVozDTO>(ex.Message);
      }
    }

    public async Task<Response<ConfiguracaoVozOpcoesDTO>> ObterOpcoesAsync()
    {
      try
      {
        var opcoes = await _repository.GetListAsync<ConfiguracaoVozOpcao, Guid>();
        var ativos = opcoes.Where(x => x.Ativo).OrderBy(x => x.Ordem).ThenBy(x => x.Descricao).ToList();

        var dto = new ConfiguracaoVozOpcoesDTO
        {
          Idiomas = ativos
            .Where(x => string.Equals(x.Tipo, "Language", StringComparison.OrdinalIgnoreCase))
            .Select(MapOpcao)
            .ToList(),
          Vozes = ativos
            .Where(x =>
              string.Equals(x.Tipo, "Voice", StringComparison.OrdinalIgnoreCase)
              || string.Equals(x.Tipo, "Tld", StringComparison.OrdinalIgnoreCase)
            )
            .Select(MapOpcao)
            .ToList(),
        };

        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoVozOpcoesDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoVozRequest request)
    {
      try
      {
        var spec = new ConfiguracaoVozPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoVoz, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
        {
          entidade = new ConfiguracaoVoz
          {
            ClinicaId = clinicaId,
          };

          AplicarConfiguracao(entidade, request);
          var criado = await _repository.CreateAsync<ConfiguracaoVoz, Guid>(entidade);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(criado.Id);
        }

        AplicarConfiguracao(entidade, request);
        var atualizado = await _repository.UpdateAsync<ConfiguracaoVoz, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(atualizado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void AplicarConfiguracao(
      ConfiguracaoVoz entidade,
      AtualizarConfiguracaoVozRequest request
    )
    {
      entidade.Ativo = request.Ativo;
      entidade.Provider = string.IsNullOrWhiteSpace(request.Provider) ? "web-speech" : request.Provider.Trim();
      entidade.IdiomaPadrao = string.IsNullOrWhiteSpace(request.IdiomaPadrao) ? "pt-PT" : request.IdiomaPadrao.Trim();
      entidade.SttIdioma = string.IsNullOrWhiteSpace(request.SttIdioma) ? entidade.IdiomaPadrao : request.SttIdioma.Trim();

      entidade.SttAtivo = request.SttAtivo;
      entidade.SttInterimResults = request.SttInterimResults;
      entidade.SttContinuous = request.SttContinuous;
      entidade.SttAutoPontuacao = request.SttAutoPontuacao;
      entidade.SttConfidenceMin = request.SttConfidenceMin;
      entidade.SttSilenceTimeoutMs = request.SttSilenceTimeoutMs > 0 ? request.SttSilenceTimeoutMs : 2500;
      entidade.SttMaxAlternatives = request.SttMaxAlternatives > 0 ? request.SttMaxAlternatives : 1;
      entidade.SttProfanityFilter = request.SttProfanityFilter;

      entidade.TtsAtivo = request.TtsAtivo;
      entidade.TtsVoice = string.IsNullOrWhiteSpace(request.TtsVoice) ? null : request.TtsVoice.Trim();
      entidade.TtsRate = request.TtsRate;
      entidade.TtsPitch = request.TtsPitch;
      entidade.TtsVolume = request.TtsVolume;

      entidade.TimeoutMs = request.TimeoutMs;
      entidade.MaxDuracaoCapturaSegundos = request.MaxDuracaoCapturaSegundos;
    }

    private static ConfiguracaoVozDTO MapToDTO(ConfiguracaoVoz entidade)
    {
      return new ConfiguracaoVozDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        Ativo = entidade.Ativo,
        Provider = entidade.Provider,
        IdiomaPadrao = entidade.IdiomaPadrao,
        SttIdioma = entidade.SttIdioma,
        SttAtivo = entidade.SttAtivo,
        SttInterimResults = entidade.SttInterimResults,
        SttContinuous = entidade.SttContinuous,
        SttAutoPontuacao = entidade.SttAutoPontuacao,
        SttConfidenceMin = entidade.SttConfidenceMin,
        SttSilenceTimeoutMs = entidade.SttSilenceTimeoutMs,
        SttMaxAlternatives = entidade.SttMaxAlternatives,
        SttProfanityFilter = entidade.SttProfanityFilter,
        TtsAtivo = entidade.TtsAtivo,
        TtsVoice = entidade.TtsVoice,
        TtsRate = entidade.TtsRate,
        TtsPitch = entidade.TtsPitch,
        TtsVolume = entidade.TtsVolume,
        TimeoutMs = entidade.TimeoutMs,
        MaxDuracaoCapturaSegundos = entidade.MaxDuracaoCapturaSegundos,
      };
    }

    private static ConfiguracaoVozOpcaoDTO MapOpcao(ConfiguracaoVozOpcao entidade)
    {
      return new ConfiguracaoVozOpcaoDTO
      {
        Tipo = entidade.Tipo,
        Codigo = entidade.Codigo,
        Descricao = entidade.Descricao,
        Ordem = entidade.Ordem,
      };
    }
  }
}
