using System;
using System.Linq;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Core.ChamadaVozService.DTOs;
using CliCloud.Application.Services.Core.ChamadaVozService.Specifications;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ChamadaVozService
{
  public class ServicoChamadaVoz(IRepositoryAsync repository) : IServicoChamadaVoz
  {
    private readonly IRepositoryAsync _repository = repository;

    public async Task<Response<ConfiguracaoChamadaVozDTO>> ObterConfiguracaoAtualAsync(Guid clinicaId)
    {
      try
      {
        var spec = new ConfiguracaoChamadaVozPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoChamadaVoz, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
          return ResponseFactory.Fail<ConfiguracaoChamadaVozDTO>("Configuração de chamada de voz não encontrada.");

        return ResponseFactory.Success(MapToDTO(entidade));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoChamadaVozDTO>(ex.Message);
      }
    }

    public async Task<Response<ConfiguracaoChamadaVozOpcoesDTO>> ObterOpcoesAsync()
    {
      try
      {
        var opcoes = await _repository.GetListAsync<ConfiguracaoChamadaVozOpcao, Guid>();
        var ativos = opcoes.Where(x => x.Ativo).OrderBy(x => x.Ordem).ThenBy(x => x.Descricao).ToList();

        var dto = new ConfiguracaoChamadaVozOpcoesDTO
        {
          Idiomas = ativos
            .Where(x => string.Equals(x.Tipo, "Language", StringComparison.OrdinalIgnoreCase))
            .Select(MapOpcao)
            .ToList(),
          Variacoes = ativos
            .Where(x => string.Equals(x.Tipo, "Tld", StringComparison.OrdinalIgnoreCase))
            .Select(MapOpcao)
            .ToList(),
        };

        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConfiguracaoChamadaVozOpcoesDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> GuardarConfiguracaoAsync(Guid clinicaId, AtualizarConfiguracaoChamadaVozRequest request)
    {
      try
      {
        var spec = new ConfiguracaoChamadaVozPorClinicaSpec(clinicaId);
        var entidade = (await _repository.GetListAsync<ConfiguracaoChamadaVoz, Guid>(spec)).FirstOrDefault();

        if (entidade == null)
        {
          entidade = new ConfiguracaoChamadaVoz
          {
            ClinicaId = clinicaId,
          };

          AplicarConfiguracao(entidade, request);
          var criado = await _repository.CreateAsync<ConfiguracaoChamadaVoz, Guid>(entidade);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Success(criado.Id);
        }

        AplicarConfiguracao(entidade, request);
        var atualizado = await _repository.UpdateAsync<ConfiguracaoChamadaVoz, Guid>(entidade);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(atualizado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private static void AplicarConfiguracao(
      ConfiguracaoChamadaVoz entidade,
      AtualizarConfiguracaoChamadaVozRequest request
    )
    {
      entidade.Ativo = request.Ativo;
      entidade.Url = string.IsNullOrWhiteSpace(request.Url) ? null : request.Url.Trim();
      entidade.Language = string.IsNullOrWhiteSpace(request.Language) ? "pt" : request.Language.Trim();
      entidade.Tld = string.IsNullOrWhiteSpace(request.Tld) ? "pt" : request.Tld.Trim();
    }

    private static ConfiguracaoChamadaVozDTO MapToDTO(ConfiguracaoChamadaVoz entidade)
    {
      return new ConfiguracaoChamadaVozDTO
      {
        Id = entidade.Id,
        ClinicaId = entidade.ClinicaId,
        Ativo = entidade.Ativo,
        Url = entidade.Url,
        Language = entidade.Language,
        Tld = entidade.Tld,
      };
    }

    private static ConfiguracaoChamadaVozOpcaoDTO MapOpcao(ConfiguracaoChamadaVozOpcao entidade)
    {
      return new ConfiguracaoChamadaVozOpcaoDTO
      {
        Tipo = entidade.Tipo,
        Codigo = entidade.Codigo,
        Descricao = entidade.Descricao,
        Ordem = entidade.Ordem,
      };
    }
  }
}
