using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using CliCloud.Application.Services.Atestados.AtestadoService.Filters;
using CliCloud.Application.Services.Atestados.AtestadoService.Specifications;
using CliCloud.Application.Services.Atestados.SpmsCartaConducaoService;
using CliCloud.Application.Services.Core.ConfigCartaConducaoService.Specifications;
using CliCloud.Domain.Entities.Common.Configurations;
using CliCloud.Domain.Entities.CartaConducao;


namespace CliCloud.Application.Services.Atestados.AtestadoService
{
  public class AtestadoService : IAtestadoService
  {
    private static readonly HashSet<string> CategoriasExcecaoBBe = new(StringComparer.OrdinalIgnoreCase) { "B", "BE" };
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly ISpmsCartaConducaoService _spmsCartaConducaoService;

    public AtestadoService(
      IRepositoryAsync repository,
      IMapper mapper,
      ISpmsCartaConducaoService spmsCartaConducaoService
    )
    {
      _repository = repository;
      _mapper = mapper;
      _spmsCartaConducaoService = spmsCartaConducaoService;
    }

    public async Task<PaginatedResponse<AtestadoTableDTO>> GetAtestadoPaginatedAsync(AtestadoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0)
        filter.PageNumber = 1;

      string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var specification = new AtestadoSearchTable(filter.Filters ?? [], dynamicOrder);
      PaginatedResponse<AtestadoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Atestado, AtestadoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
      return pagedResponse;
    }

    public async Task<Response<IEnumerable<AtestadoTableDTO>>> GetAllAtestadoAsync(AtestadoAllFilter filter)
    {
      try
      {
        filter ??= new AtestadoAllFilter();
        string dynamicOrder = filter.GetOrderByString();
        List<TableFilter> tableFilters = filter.Filters ?? [];
        var specification = new AtestadoSearchTable(tableFilters, dynamicOrder);
        IEnumerable<AtestadoTableDTO> list = await _repository.GetListAsync<Atestado, AtestadoTableDTO, Guid>(specification);
        return ResponseFactory.Success<IEnumerable<AtestadoTableDTO>>(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AtestadoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<AtestadoDTO>> GetAtestadoAsync(Guid id)
    {
      try
      {
        AtestadoDTO dto = await _repository.GetByIdAsync<Atestado, AtestadoDTO, Guid>(id);
        return ResponseFactory.Success<AtestadoDTO>(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<AtestadoDTO>(ex.Message);
      }
    }

    public async Task<Response<string?>> ObterErroComunicacaoAsync(Guid id, Guid clinicaId)
    {
      try
      {
        var atestado = await _repository.GetByIdAsync<Atestado, Guid>(id);
        if (atestado == null)
          return ResponseFactory.Fail<string?>("Atestado não encontrado.");
        if (atestado.ClinicaId != clinicaId)
          return ResponseFactory.Fail<string?>("Atestado não pertence à clínica atual.");

        return ResponseFactory.Success(atestado.MensagemErro);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<string?>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAtestadoAsync(CreateAtestadoRequest request)
    {
      Atestado atestado = _mapper.Map<CreateAtestadoRequest, Atestado>(request);
      atestado.Id = Guid.NewGuid();
      atestado.EstadoEnvio = 0;

      try
      {
        await NormalizarCategoriasBBeAsync(request);

        _ = await _repository.CreateAsync<Atestado, Guid>(atestado);

        foreach (var item in request.Categorias)
        {
          var cat = new AtestadoCategoria
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoId = item.CartaConducaoId,
            Apto = item.Apto,
            AptoGrupo2 = item.AptoGrupo2
          };
          _ = await _repository.CreateAsync<AtestadoCategoria, Guid>(cat);
        }

        foreach (var item in request.Restricoes)
        {
          var rest = new AtestadoRestricao
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoRestricaoId = item.CartaConducaoRestricaoId,
            CartaConducaoId = item.CartaConducaoId,
            Anotacoes = item.Anotacoes
          };
          _ = await _repository.CreateAsync<AtestadoRestricao, Guid>(rest);
        }

        foreach (var item in request.RestricoesAnteriores)
        {
          var restAnt = new AtestadoRestricaoAnterior
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoRestricaoId = item.CartaConducaoRestricaoId,
            Anotacoes = item.Anotacoes
          };
          _ = await _repository.CreateAsync<AtestadoRestricaoAnterior, Guid>(restAnt);
        }

        _ = await _repository.SaveChangesAsync();

        var atestadoCriado = (await _repository.GetListAsync<Atestado, Guid>(
          new AtestadoByIdForSpmsSpec(atestado.Id)
        )).FirstOrDefault();

        if (atestadoCriado == null)
          return ResponseFactory.Fail<Guid>("Atestado criado mas não foi possível recarregar para comunicação SPMS.");

        var cfgSpec = new ConfigCartaConducaoPorClinicaSpec(atestadoCriado.ClinicaId);
        var cfg = (await _repository.GetListAsync<ConfigCartaConducao, Guid>(cfgSpec)).FirstOrDefault();

        if (cfg == null)
        {
          atestadoCriado.EstadoEnvio = 2;
          atestadoCriado.MensagemErro = "Configuração de carta de condução não encontrada para a clínica.";
          _ = await _repository.UpdateAsync<Atestado, Guid>(atestadoCriado);
          await _repository.SaveChangesAsync();
          return ResponseFactory.Fail<Guid>("Configuração de carta de condução não encontrada para a clínica.");
        }

        var resultadoComunicacao = await ComunicarAtestadoAsync(atestadoCriado, cfg, useOfflineEndpoint: false);
        if (!resultadoComunicacao.Status)
          return ResponseFactory.Fail<Guid>(resultadoComunicacao.Mensagem ?? "Falha na comunicação SPMS.");

        return ResponseFactory.Success<Guid>(atestadoCriado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private async Task NormalizarCategoriasBBeAsync(CreateAtestadoRequest request)
    {
      foreach (var categoria in request.Categorias)
      {
        var carta = await _repository.GetByIdAsync<CartaConducao, Guid>(categoria.CartaConducaoId);
        var codigo = carta?.CodigoCarta?.Trim();
        if (string.IsNullOrWhiteSpace(codigo) || !CategoriasExcecaoBBe.Contains(codigo))
          continue;

        if (categoria.Apto == 0 && categoria.AptoGrupo2 == 1)
          categoria.Apto = 1;
      }
    }

    public async Task<Response<Guid>> ReenviarAtestadoOfflineAsync(Guid id, Guid clinicaId)
    {
      try
      {
        var atestado = (await _repository.GetListAsync<Atestado, Guid>(new AtestadoByIdForSpmsSpec(id))).FirstOrDefault();
        if (atestado == null)
          return ResponseFactory.Fail<Guid>("Atestado não encontrado.");
        if (atestado.ClinicaId != clinicaId)
          return ResponseFactory.Fail<Guid>("Atestado não pertence à clínica atual.");

        var cfgSpec = new ConfigCartaConducaoPorClinicaSpec(atestado.ClinicaId);
        var cfg = (await _repository.GetListAsync<ConfigCartaConducao, Guid>(cfgSpec)).FirstOrDefault();
        if (cfg == null)
          return ResponseFactory.Fail<Guid>("Configuração de carta de condução não encontrada para a clínica.");

        var resultadoComunicacao = await ComunicarAtestadoAsync(atestado, cfg, useOfflineEndpoint: true);
        if (!resultadoComunicacao.Status)
          return ResponseFactory.Fail<Guid>(resultadoComunicacao.Mensagem ?? "Falha no reenvio offline SPMS.");

        return ResponseFactory.Success(id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<int>> ReenviarPendentesOfflineAsync(Guid clinicaId)
    {
      try
      {
        var pendentes = (await _repository.GetListAsync<Atestado, Guid>(new AtestadoPendentesForSpmsSpec(clinicaId))).ToList();
        if (pendentes.Count == 0)
          return ResponseFactory.Success(0);
        var cfgSpec = new ConfigCartaConducaoPorClinicaSpec(clinicaId);
        var cfg = (await _repository.GetListAsync<ConfigCartaConducao, Guid>(cfgSpec)).FirstOrDefault();
        if (cfg == null)
          return ResponseFactory.Fail<int>("Configuração de carta de condução não encontrada para a clínica.");

        var sucesso = 0;
        foreach (var atestado in pendentes)
        {
          var resultadoComunicacao = await ComunicarAtestadoAsync(atestado, cfg, useOfflineEndpoint: true);
          if (resultadoComunicacao.Status)
            sucesso++;
        }

        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(sucesso);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<int>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteAtestadoAsync(Guid id)
    {
      try
      {
        var categorias = await _repository.GetListAsync<AtestadoCategoria, Guid>(new AtestadoCategoriaByAtestadoId(id));
        foreach (var c in categorias)
          await _repository.RemoveAsync<AtestadoCategoria, Guid>(c);

        var restricoes = await _repository.GetListAsync<AtestadoRestricao, Guid>(new AtestadoRestricaoByAtestadoId(id));
        foreach (var r in restricoes)
          await _repository.RemoveAsync<AtestadoRestricao, Guid>(r);

        var restricoesAnt = await _repository.GetListAsync<AtestadoRestricaoAnterior, Guid>(new AtestadoRestricaoAnteriorByAtestadoId(id));
        foreach (var ra in restricoesAnt)
          await _repository.RemoveAsync<AtestadoRestricaoAnterior, Guid>(ra);

        Atestado? atestado = await _repository.RemoveByIdAsync<Atestado, Guid>(id);
        if (atestado == null)
          return ResponseFactory.Fail<Guid>("Atestado não encontrado");
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(atestado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    private async Task<(bool Status, string? Mensagem)> ComunicarAtestadoAsync(
      Atestado atestado,
      ConfigCartaConducao cfg,
      bool useOfflineEndpoint
    )
    {
      if (atestado.Utente == null || atestado.Medico == null || atestado.Clinica == null)
        return (false, "Atestado sem dados obrigatórios para comunicação SPMS.");

      var spms = useOfflineEndpoint
        ? await _spmsCartaConducaoService.RegistarOfflineAsync(
          atestado,
          atestado.Utente,
          atestado.Medico,
          atestado.Clinica,
          cfg,
          atestado.Categorias.ToList(),
          atestado.Restricoes.ToList(),
          atestado.RestricoesAnteriores.ToList())
        : await _spmsCartaConducaoService.RegistarOnlineAsync(
          atestado,
          atestado.Utente,
          atestado.Medico,
          atestado.Clinica,
          cfg,
          atestado.Categorias.ToList(),
          atestado.Restricoes.ToList(),
          atestado.RestricoesAnteriores.ToList());

      if (!spms.Success)
      {
        await AtualizarEstadoComunicacaoAsync(
          atestado,
          sucesso: false,
          isTransientFailure: spms.IsTransientFailure,
          mensagem: spms.Message
        );
        
        var prefix = spms.IsTransientFailure
          ? "Comunicação SPMS indisponível (pendente para reenvio): "
          : "Falha funcional na comunicação SPMS: ";
        return (false, $"{prefix}{spms.Message}");
      }

      await AtualizarEstadoComunicacaoAsync(
        atestado,
        sucesso: true,
        isTransientFailure: false,
        mensagem: null,
        numeroSpms: spms.NumeroAtestadoMedico
      );

      return (true, null);
    }

    private async Task AtualizarEstadoComunicacaoAsync(
      Atestado atestado,
      bool sucesso, 
      bool isTransientFailure,
      string? mensagem,
      string? numeroSpms = null
    )
    {
      if (sucesso)
      {
        atestado.EstadoEnvio = 1;
        atestado.DataEnvio = DateTime.Now;
        atestado.NumeroSPMS = numeroSpms;
        atestado.MensagemErro = null;
      }
      else
      {
        atestado.EstadoEnvio = isTransientFailure ? 0 : 2;
        atestado.DataEnvio = null;
        atestado.MensagemErro = mensagem;
      }

      _ = await _repository.UpdateAsync<Atestado, Guid>(atestado);
      await _repository.SaveChangesAsync();
    }
  }
}
