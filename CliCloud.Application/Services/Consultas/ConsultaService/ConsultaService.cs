using AutoMapper;
using System.Globalization;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Consultas.ConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ConsultaService.Filters;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Application.Services.Utentes.UtenteService.DTOs;
using CliCloud.Application.Services.Utentes.UtenteService.Specifications;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Medicos.MedicoService;

namespace CliCloud.Application.Services.Consultas.ConsultaService
{
  public class ConsultaService : IConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentTenantUserService _currentTenantUserService;
    private readonly IMedicoService _medicoService;

    public ConsultaService(
      IRepositoryAsync repository,
      IMapper mapper,
      ICurrentTenantUserService currentTenantUserService,
      IMedicoService medicoService
    )
    {
      _repository = repository;
      _mapper = mapper;
      _currentTenantUserService = currentTenantUserService;
      _medicoService = medicoService;
    }

    // full list
    public async Task<Response<IEnumerable<ConsultaDTO>>> GetConsultaAsync(string keyword = "")
    {
      var spec = new ConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<Consulta, ConsultaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    // lightweight list
    public async Task<Response<IEnumerable<ConsultaLightDTO>>> GetConsultaLightAsync(string keyword = "")
    {
      var spec = new ConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<Consulta, ConsultaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    // paginated list (Tanstack)
    public async Task<PaginatedResponse<ConsultaTableDTO>> GetConsultaPaginatedAsync(ConsultaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;

      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      List<TableFilter> filters = await ResolveConsultaContextFiltersAsync(filter.Filters ?? []);
      var spec = new ConsultaSearchTable(filters, order);
      PaginatedResponse<ConsultaTableDTO> result = await _repository.GetPaginatedResultsAsync<
        Consulta,
        ConsultaTableDTO,
        Guid
      >(filter.PageNumber, filter.PageSize, spec);
      await HydrateConsultaUtenteNumerosAsync(result.Data);
      return result;
    }

    private async Task<List<TableFilter>> ResolveConsultaContextFiltersAsync(
      List<TableFilter> filters
    )
    {
      List<TableFilter> resolved = filters
        .Where(f => !string.Equals(f.Id, "medico_logado", StringComparison.OrdinalIgnoreCase))
        .ToList();

      bool filtrarMedicoLogado = filters.Any(f =>
        string.Equals(f.Id, "medico_logado", StringComparison.OrdinalIgnoreCase)
        && bool.TryParse(f.Value, out bool value)
        && value
      );

      if (!filtrarMedicoLogado)
      {
        return resolved;
      }

      string? userIdStr = _currentTenantUserService.UserId;
      if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
      {
        resolved.Add(new TableFilter { Id = "id", Value = Guid.Empty.ToString() });
        return resolved;
      }

      var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
      Guid? medicoId = medicoRes.Data?.Id;
      resolved.Add(
        new TableFilter
        {
          Id = "medicoid",
          Value = (medicoId ?? Guid.Empty).ToString()
        }
      );
      return resolved;
    }

    // all (non-paginated)
    public async Task<Response<IEnumerable<ConsultaTableDTO>>> GetAllConsultaAsync(ConsultaAllFilter? filter)
    {
      try
      {
        filter ??= new ConsultaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ConsultaSearchTable(filters, order);
        List<ConsultaTableDTO> list = (await _repository.GetListAsync<Consulta, ConsultaTableDTO, Guid>(
          spec
        )).ToList();
        await HydrateConsultaUtenteNumerosAsync(list);
        return ResponseFactory.Success<IEnumerable<ConsultaTableDTO>>(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ConsultaTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<ConsultaDoDiaDTO>>> GetConsultasDoDiaAsync(
      DateTime data,
      bool desmarcadas = false
    )
    {
      Guid? medicoId = await ResolveMedicoLogadoIdAsync();

      List<Admissao> admissoes = (
        await _repository.GetListAsync<Admissao, Guid>(
          new ConsultasDoDiaAdmissoesSpec(data, medicoId, desmarcadas)
        )
      ).ToList();

      List<Consulta> consultas = (
        await _repository.GetListAsync<Consulta, Guid>(
          new ConsultasDoDiaConsultasSpec(data, medicoId, desmarcadas)
        )
      ).ToList();

      List<ConsultaMarcacao> marcacoes = (
        await _repository.GetListAsync<ConsultaMarcacao, Guid>(
          new ConsultasDoDiaMarcacoesSpec(data, medicoId, desmarcadas)
        )
      ).ToList();

      HashSet<Guid> consultaIdsComAdmissaoAtiva = admissoes
        .Select(a => a.Consulta?.Id)
        .Where(id => id.HasValue)
        .Select(id => id!.Value)
        .ToHashSet();

      HashSet<Guid> marcacaoIdsComAdmissaoAtiva = admissoes
        .Select(a => a.ConsultaMarcacaoId)
        .Where(id => id.HasValue)
        .Select(id => id!.Value)
        .ToHashSet();

      HashSet<Guid> consultaIds = consultas
        .Select(c => c.Id)
        .Concat(consultaIdsComAdmissaoAtiva)
        .ToHashSet();

      HashSet<Guid> marcacaoIdsComConsulta = consultas
        .Select(c => c.ConsultaMarcacaoId)
        .Where(id => id.HasValue)
        .Select(id => id!.Value)
        .Concat(marcacaoIdsComAdmissaoAtiva)
        .ToHashSet();

      List<ConsultaDoDiaDTO> rows = admissoes
        .Select(MapAdmissaoConsultaDoDia)
        .Concat(
          consultas
            .Where(c =>
              !consultaIdsComAdmissaoAtiva.Contains(c.Id)
              && (
                !c.ConsultaMarcacaoId.HasValue
                || !marcacaoIdsComAdmissaoAtiva.Contains(c.ConsultaMarcacaoId.Value)))
            .Select(MapConsultaDoDia)
        )
        .Concat(
          marcacoes
            .Where(m =>
              !marcacaoIdsComConsulta.Contains(m.Id)
              && (!m.ConsultaId.HasValue || !consultaIds.Contains(m.ConsultaId.Value)))
            .Select(MapMarcacaoConsultaDoDia)
        )
        .OrderBy(x => x.Data ?? DateTime.MaxValue)
        .ThenBy(x => x.HoraInicio)
        .ToList();

      return ResponseFactory.Success<IEnumerable<ConsultaDoDiaDTO>>(rows);
    }

    // single by id
    public async Task<Response<ConsultaDTO>> GetConsultaAsync(Guid id)
    {
      try
      {
        var spec = new ConsultaByIdWithIncludes(id);
        var dto = await _repository.GetByIdAsync<Consulta, ConsultaDTO, Guid>(id, spec);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ConsultaDTO>(ex.Message);
      }
    }

    // create
    public async Task<Response<Guid>> CreateConsultaAsync(CreateConsultaRequest request)
    {
      var entity = _mapper.Map<Consulta>(request);
      try
      {
        var created = await _repository.CreateAsync<Consulta, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    // create from ConsultaMarcacao (agenda -> ato clínico)
    public async Task<Response<Guid>> CreateConsultaFromMarcacaoAsync(Guid marcacaoId)
    {
      Response<IniciarAtendimentoConsultaDTO> result = await IniciarAtendimentoAsync(
        new IniciarAtendimentoConsultaRequest { ConsultaMarcacaoId = marcacaoId }
      );

      return result.Status == ResponseStatus.Success && result.Data != null
        ? ResponseFactory.Success(result.Data.ConsultaId)
        : ResponseFactory.Fail<Guid>(FirstMessage(result.Messages) ?? "Não foi possível iniciar o atendimento.");
    }

    public async Task<Response<IniciarAtendimentoConsultaDTO>> IniciarAtendimentoAsync(
      IniciarAtendimentoConsultaRequest request
    )
    {
      try
      {
        if (!request.ConsultaId.HasValue
          && !request.ConsultaMarcacaoId.HasValue
          && !request.AdmissaoId.HasValue)
        {
          return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
            "Indique a consulta, admissão ou marcação a atender."
          );
        }

        bool consultaCriada = false;
        string origem = "Consulta";

        Consulta? consulta = request.ConsultaId.HasValue
          ? await _repository.GetByIdAsync<Consulta, Guid>(request.ConsultaId.Value)
          : null;

        Admissao? admissao = null;
        ConsultaMarcacao? marcacao = null;

        if (request.AdmissaoId.HasValue)
        {
          admissao = await ObterAdmissaoComServicosAsync(request.AdmissaoId.Value);
          origem = "Admissao";
        }

        if (request.ConsultaMarcacaoId.HasValue)
        {
          marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
            request.ConsultaMarcacaoId.Value
          );
          if (marcacao == null || marcacao.DeletedOn != null)
          {
            return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
              "Marcação de consulta não encontrada."
            );
          }

          origem = admissao != null ? origem : "Marcacao";
        }

        if (consulta == null && admissao != null)
        {
          consulta = await ObterConsultaAssociadaAsync(admissao);
        }

        if (consulta == null && marcacao?.ConsultaId.HasValue == true)
        {
          consulta = await _repository.GetByIdAsync<Consulta, Guid>(marcacao.ConsultaId.Value);
        }

        if (admissao == null && marcacao != null)
        {
          admissao = await ObterAdmissaoPorMarcacaoComServicosAsync(marcacao.Id);
        }

        if (admissao != null && !PodeIniciarAtendimento(admissao.StatusConsulta))
        {
          return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
            "Não é possível iniciar atendimento para uma admissão desmarcada, suspensa ou concluída."
          );
        }

        if (marcacao != null && !PodeIniciarAtendimento(marcacao.StatusConsulta))
        {
          return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
            "Não é possível iniciar atendimento para uma marcação desmarcada, suspensa ou concluída."
          );
        }

        if (consulta == null)
        {
          if (admissao != null)
          {
            consulta = await CriarConsultaClinicaDesdeAdmissaoAsync(admissao);
            consultaCriada = true;
          }
          else if (marcacao != null)
          {
            consulta = await CriarConsultaClinicaDesdeMarcacaoAsync(marcacao);
            consultaCriada = true;
          }
        }

        if (consulta == null || consulta.DeletedOn != null)
        {
          return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
            "Consulta não encontrada."
          );
        }

        if (!PodeIniciarAtendimento(consulta.StatusConsulta))
        {
          return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(
            "Não é possível iniciar atendimento para uma consulta desmarcada, suspensa ou concluída."
          );
        }

        if (admissao == null && consulta.AdmissaoId.HasValue)
        {
          admissao = await ObterAdmissaoComServicosAsync(consulta.AdmissaoId.Value);
        }

        if (marcacao == null && consulta.ConsultaMarcacaoId.HasValue)
        {
          marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
            consulta.ConsultaMarcacaoId.Value
          );
        }

        if (admissao != null)
        {
          await SincronizarServicosAdmissaoNaConsultaAsync(admissao, consulta.Id);
        }

        await AplicarEstadoEmAtendimentoAsync(consulta, admissao, marcacao);
        _ = await _repository.SaveChangesAsync();

        return ResponseFactory.Success(await MapAtendimentoContextoAsync(
          consulta,
          admissao,
          marcacao,
          origem,
          consultaCriada
        ));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IniciarAtendimentoConsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> FinalizarConsultaAsync(Guid id)
    {
      try
      {
        var horaFim = DateTime.Now.TimeOfDay;
        var consulta = await _repository.GetByIdAsync<Consulta, Guid>(id);
        if (consulta == null || consulta.DeletedOn != null)
          return ResponseFactory.Fail<Guid>("Consulta não encontrada.");

        if (consulta.StatusConsulta
            is StatusConsulta.Desmarcada
            or StatusConsulta.Suspensa
            or StatusConsulta.Faltou
            or StatusConsulta.FaltouJustificada)
        {
          return ResponseFactory.Fail<Guid>(
            "Não é possível concluir uma consulta desmarcada, suspensa ou marcada como falta."
          );
        }

        Admissao? admissao = await ObterAdmissaoAssociadaComServicosAsync(consulta);
        if (admissao != null)
        {
          AdmissaoPromocaoHelper.MesclarAdmissaoEmConsultaExistente(consulta, admissao);
          await SincronizarServicosAdmissaoNaConsultaAsync(admissao, consulta.Id);
        }

        consulta.StatusConsulta = StatusConsulta.Concluida;
        consulta.Efetuado = true;
        consulta.Faltou = false;
        consulta.Confirmado ??= admissao?.Confirmado ?? true;
        consulta.ConfirmaConsulta ??= admissao?.ConfirmaConsulta;
        consulta.HoraChegada ??= admissao?.HoraChegada;
        consulta.AdmissaoId ??= admissao?.Id;
        consulta.HoraFim = horaFim;
        _ = await _repository.UpdateAsync<Consulta, Guid>(consulta);

        var marcacoes = await _repository.GetListAsync<ConsultaMarcacao, Guid>(
          new ConsultaMarcacoesByConsultaIdSpec(id)
        );
        foreach (var marcacao in marcacoes)
        {
          marcacao.ConsultaId = id;
          marcacao.StatusConsulta = StatusConsulta.Concluida;
          marcacao.EmTratamento = false;
          _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
        }

        if (admissao != null && admissao.DeletedOn == null)
        {
          admissao.Efetuado = true;
          admissao.StatusConsulta = StatusConsulta.Concluida;
          admissao.EmTratamento = false;
          admissao.Confirmado ??= true;
          admissao.HoraFim = horaFim;
          _ = await _repository.UpdateAsync<Admissao, Guid>(admissao);
        }

        _ = await _repository.SaveChangesAsync();

        // Criar registo de faturação se ainda não existir
        var existeFaturacao = await _repository.ExistsAsync<ConsultaFaturacao, Guid>(
          new ConsultaFaturacaoByConsultaId(id)
        );

        if (!existeFaturacao)
        {
          var faturacao = new ConsultaFaturacao
          {
            ConsultaId = id,
            Pago = false,
            Faturado = false,
          };

          _ = await _repository.CreateAsync<ConsultaFaturacao, Guid>(faturacao);
          _ = await _repository.SaveChangesAsync();
        }

        return ResponseFactory.Success(id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    // update
    public async Task<Response<Guid>> UpdateConsultaAsync(UpdateConsultaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Consulta, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Consulta não encontrada.");

      _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<Consulta, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    // delete
    public async Task<Response<Guid>> DeleteConsultaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Consulta, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    // delete multiple
    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleConsultaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var entity = await _repository.GetByIdAsync<Consulta, Guid>(id);
          if (entity == null) { fail.Add($"Consulta {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Consulta, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Consulta {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    private async Task<Admissao?> ObterAdmissaoComServicosAsync(Guid admissaoId)
    {
      return (
        await _repository.GetListAsync<Admissao, Guid>(
          new AdmissaoByIdWithServicosSpec(admissaoId)
        )
      ).FirstOrDefault();
    }

    private async Task<Admissao?> ObterAdmissaoPorMarcacaoComServicosAsync(Guid marcacaoId)
    {
      Admissao? admissao = (
        await _repository.GetListAsync<Admissao, Guid>(
          new AdmissaoByConsultaMarcacaoSpec(marcacaoId)
        )
      ).FirstOrDefault();

      return admissao == null ? null : await ObterAdmissaoComServicosAsync(admissao.Id);
    }

    private async Task<Admissao?> ObterAdmissaoAssociadaComServicosAsync(Consulta consulta)
    {
      if (consulta.AdmissaoId.HasValue)
      {
        Admissao? admissao = await ObterAdmissaoComServicosAsync(consulta.AdmissaoId.Value);
        if (admissao != null)
        {
          return admissao;
        }
      }

      if (!consulta.ConsultaMarcacaoId.HasValue)
      {
        return null;
      }

      return await ObterAdmissaoPorMarcacaoComServicosAsync(consulta.ConsultaMarcacaoId.Value);
    }

    private async Task<Consulta?> ObterConsultaAssociadaAsync(Admissao admissao)
    {
      Consulta? consultaPorAdmissao = (
        await _repository.GetListAsync<Consulta, Guid>(
          new ConsultaPorAdmissaoSpec(admissao.Id)
        )
      ).FirstOrDefault();
      if (consultaPorAdmissao != null)
      {
        return consultaPorAdmissao;
      }

      if (!admissao.ConsultaMarcacaoId.HasValue)
      {
        return null;
      }

      ConsultaMarcacao marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
        admissao.ConsultaMarcacaoId.Value
      );
      if (marcacao == null || marcacao.DeletedOn != null || !marcacao.ConsultaId.HasValue)
      {
        return null;
      }

      Consulta consulta = await _repository.GetByIdAsync<Consulta, Guid>(marcacao.ConsultaId.Value);
      return consulta?.DeletedOn == null ? consulta : null;
    }

    private async Task<Consulta> CriarConsultaClinicaDesdeAdmissaoAsync(Admissao admissao)
    {
      Consulta consulta = AdmissaoPromocaoHelper.CriarConsultaDesdeAdmissao(admissao);
      PrepararConsultaParaAtendimento(consulta);

      Consulta created = await _repository.CreateAsync<Consulta, Guid>(consulta);
      foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicos(admissao, created.Id))
      {
        if (servico.Id == Guid.Empty)
        {
          servico.Id = Guid.NewGuid();
        }

        _ = await _repository.CreateAsync<ServicoConsulta, Guid>(servico);
      }

      if (admissao.ConsultaMarcacaoId.HasValue)
      {
        ConsultaMarcacao marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(
          admissao.ConsultaMarcacaoId.Value
        );
        if (marcacao != null && marcacao.DeletedOn == null)
        {
          marcacao.ConsultaId = created.Id;
          _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
        }
      }

      return created;
    }

    private async Task<Consulta> CriarConsultaClinicaDesdeMarcacaoAsync(ConsultaMarcacao marcacao)
    {
      Guid? medicoId = marcacao.MedicoId ?? await ResolveMedicoLogadoIdAsync();
      if (medicoId.HasValue && marcacao.MedicoId == null)
      {
        marcacao.MedicoId = medicoId;
      }

      Consulta consulta = new()
      {
        UtenteId = marcacao.UtenteId,
        MedicoId = medicoId,
        EspecialidadeId = marcacao.EspecialidadeId,
        TecnicoId = marcacao.TecnicoId,
        FuncionarioId = marcacao.FuncionarioId,
        SalaId = marcacao.SalaId,
        MedicoExternoId = marcacao.MedicoExternoId,
        Data = marcacao.Data,
        HoraInicio = marcacao.HoraMarcacao,
        StatusConsulta = StatusConsulta.EmAtendimento,
        ConsultaMarcacaoId = marcacao.Id,
        TipoAdmissaoId = marcacao.TipoAdmissaoId,
        TipoConsultaId = marcacao.TipoConsultaId,
        MotivoConsultaId = marcacao.MotivoConsultaId,
        NumDestacavel = marcacao.NumDestacavel,
        Obs = marcacao.Obs,
        Efetuado = false,
        Faltou = false,
      };

      Consulta created = await _repository.CreateAsync<Consulta, Guid>(consulta);
      marcacao.ConsultaId = created.Id;
      _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
      return created;
    }

    private async Task SincronizarServicosAdmissaoNaConsultaAsync(Admissao admissao, Guid consultaId)
    {
      List<ServicoConsulta> servicosExistentes = (
        await _repository.GetListAsync<ServicoConsulta, Guid>(
          new ServicosConsultaPorConsultaIdSpec(consultaId)
        )
      ).ToList();

      List<int> linhas = servicosExistentes.Select(s => s.Linha).ToList();
      foreach (ServicoConsulta servico in AdmissaoPromocaoHelper.MapearServicosNovos(
                 admissao,
                 consultaId,
                 linhas
               ))
      {
        if (servico.Id == Guid.Empty)
        {
          servico.Id = Guid.NewGuid();
        }

        _ = await _repository.CreateAsync<ServicoConsulta, Guid>(servico);
      }
    }

    private static bool PodeIniciarAtendimento(StatusConsulta? status)
    {
      return status
        is null
        or StatusConsulta.Agendada
        or StatusConsulta.Pendente
        or StatusConsulta.EmAtendimento;
    }

    private static void PrepararConsultaParaAtendimento(Consulta consulta)
    {
      consulta.StatusConsulta = StatusConsulta.EmAtendimento;
      consulta.Efetuado = false;
      consulta.Faltou = false;
      consulta.HoraFim = null;
    }

    private async Task AplicarEstadoEmAtendimentoAsync(
      Consulta consulta,
      Admissao? admissao,
      ConsultaMarcacao? marcacao
    )
    {
      PrepararConsultaParaAtendimento(consulta);
      _ = await _repository.UpdateAsync<Consulta, Guid>(consulta);

      if (admissao != null && admissao.DeletedOn == null)
      {
        admissao.StatusConsulta = StatusConsulta.EmAtendimento;
        admissao.EmTratamento = true;
        admissao.Efetuado = false;
        _ = await _repository.UpdateAsync<Admissao, Guid>(admissao);
      }

      if (marcacao != null && marcacao.DeletedOn == null)
      {
        marcacao.ConsultaId = consulta.Id;
        marcacao.StatusConsulta = StatusConsulta.EmAtendimento;
        marcacao.EmTratamento = true;
        _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
      }
    }

    private async Task<IniciarAtendimentoConsultaDTO> MapAtendimentoContextoAsync(
      Consulta consulta,
      Admissao? admissao,
      ConsultaMarcacao? marcacao,
      string origem,
      bool consultaCriada
    )
    {
      string? utenteNome = consulta.Utente?.Nome ?? admissao?.Utente?.Nome;
      if (string.IsNullOrWhiteSpace(utenteNome) && consulta.UtenteId.HasValue)
      {
        var utente = await _repository.GetByIdAsync<Utente, Guid>(consulta.UtenteId.Value);
        utenteNome = utente?.Nome;
      }

      return new IniciarAtendimentoConsultaDTO
      {
        ConsultaId = consulta.Id,
        ConsultaMarcacaoId = consulta.ConsultaMarcacaoId ?? marcacao?.Id,
        AdmissaoId = consulta.AdmissaoId ?? admissao?.Id,
        UtenteId = consulta.UtenteId ?? admissao?.UtenteId ?? marcacao?.UtenteId ?? Guid.Empty,
        MedicoId = consulta.MedicoId ?? admissao?.MedicoId ?? marcacao?.MedicoId,
        UtenteNome = utenteNome,
        Origem = origem,
        ConsultaCriada = consultaCriada,
      };
    }

    private static string? FirstMessage(Dictionary<string, List<string>> messages)
    {
      return messages.Values.SelectMany(x => x).FirstOrDefault();
    }

    private async Task<Guid?> ResolveMedicoLogadoIdAsync()
    {
      string? userIdStr = _currentTenantUserService.UserId;
      if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
      {
        return null;
      }

      var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
      return medicoRes.Status == ResponseStatus.Success ? medicoRes.Data?.Id : null;
    }

    private static ConsultaDoDiaDTO MapAdmissaoConsultaDoDia(Admissao admissao)
    {
      Guid? consultaId = admissao.Consulta?.Id ?? admissao.ConsultaMarcacao?.ConsultaId;

      return new ConsultaDoDiaDTO
      {
        Id = admissao.Id,
        Origem = "Admissao",
        AdmissaoId = admissao.Id,
        ConsultaId = consultaId,
        ConsultaMarcacaoId = admissao.ConsultaMarcacaoId,
        UtenteId = admissao.UtenteId,
        UtenteNumero = admissao.Utente?.NumeroUtente,
        UtenteNome = admissao.Utente?.Nome,
        MedicoId = admissao.MedicoId,
        MedicoNome = admissao.Medico?.Nome,
        EspecialidadeId = admissao.EspecialidadeId,
        EspecialidadeDesignacao = admissao.Especialidade?.Nome,
        OrganismoId = admissao.OrganismoId,
        OrganismoNome = admissao.Organismo?.Nome,
        Data = admissao.Data,
        DataLabel = FormatDate(admissao.Data),
        HoraInicio = FormatTime(admissao.HoraInicio),
        HoraFim = FormatTime(admissao.HoraFim),
        HoraChegada = FormatTime(admissao.HoraChegada),
        TipoConsultaId = admissao.TipoConsultaId,
        TipoConsultaDesignacao = admissao.TipoConsultaItem?.Designacao,
        TipoAdmissaoId = admissao.TipoAdmissaoId,
        TipoAdmissaoDesignacao = admissao.TipoAdmissao?.Designacao,
        Diagnostico = admissao.Diagnostico,
        StatusConsulta = ToInt(admissao.StatusConsulta),
        StatusConsultaLabel = ResolveStatusLabel(
          admissao.StatusConsulta,
          admissao.Confirmado,
          admissao.Efetuado,
          null
        ),
        Confirmado = admissao.Confirmado,
        Efetuado = admissao.Efetuado,
      };
    }

    private static ConsultaDoDiaDTO MapConsultaDoDia(Consulta consulta)
    {
      return new ConsultaDoDiaDTO
      {
        Id = consulta.Id,
        Origem = "Consulta",
        ConsultaId = consulta.Id,
        ConsultaMarcacaoId = consulta.ConsultaMarcacaoId,
        AdmissaoId = consulta.AdmissaoId,
        UtenteId = consulta.UtenteId,
        UtenteNumero = consulta.Utente?.NumeroUtente,
        UtenteNome = consulta.Utente?.Nome,
        MedicoId = consulta.MedicoId,
        MedicoNome = consulta.Medico?.Nome,
        EspecialidadeId = consulta.EspecialidadeId,
        EspecialidadeDesignacao = consulta.Especialidade?.Nome,
        OrganismoId = consulta.OrganismoId,
        OrganismoNome = consulta.Organismo?.Nome,
        Data = consulta.Data,
        DataLabel = FormatDate(consulta.Data),
        HoraInicio = FormatTime(consulta.HoraInicio),
        HoraFim = FormatTime(consulta.HoraFim),
        HoraChegada = FormatTime(consulta.HoraChegada),
        TipoConsultaId = consulta.TipoConsultaId,
        TipoConsultaDesignacao = consulta.TipoConsultaItem?.Designacao,
        TipoAdmissaoId = consulta.TipoAdmissaoId,
        TipoAdmissaoDesignacao = consulta.TipoAdmissao?.Designacao,
        Diagnostico = consulta.Diagnostico,
        StatusConsulta = ToInt(consulta.StatusConsulta),
        StatusConsultaLabel = ResolveStatusLabel(
          consulta.StatusConsulta,
          consulta.Confirmado,
          consulta.Efetuado,
          consulta.Faltou
        ),
        Confirmado = consulta.Confirmado,
        Efetuado = consulta.Efetuado,
        Faltou = consulta.Faltou,
      };
    }

    private static ConsultaDoDiaDTO MapMarcacaoConsultaDoDia(ConsultaMarcacao marcacao)
    {
      return new ConsultaDoDiaDTO
      {
        Id = marcacao.Id,
        Origem = "ConsultaMarcacao",
        ConsultaId = marcacao.ConsultaId,
        ConsultaMarcacaoId = marcacao.Id,
        AdmissaoId = null,
        UtenteId = marcacao.UtenteId,
        UtenteNumero = marcacao.Utente?.NumeroUtente,
        UtenteNome = marcacao.Utente?.Nome,
        MedicoId = marcacao.MedicoId,
        MedicoNome = marcacao.Medico?.Nome,
        EspecialidadeId = marcacao.EspecialidadeId,
        EspecialidadeDesignacao = marcacao.Especialidade?.Nome,
        OrganismoId = marcacao.Utente?.OrganismoId,
        OrganismoNome = marcacao.Utente?.Organismo?.Nome,
        Data = marcacao.Data,
        DataLabel = FormatDate(marcacao.Data),
        HoraInicio = FormatTime(marcacao.HoraMarcacao),
        TipoConsultaId = marcacao.TipoConsultaId,
        TipoConsultaDesignacao = marcacao.TipoConsultaItem?.Designacao,
        TipoAdmissaoId = marcacao.TipoAdmissaoId,
        TipoAdmissaoDesignacao = marcacao.TipoAdmissao?.Designacao,
        StatusConsulta = ToInt(marcacao.StatusConsulta),
        StatusConsultaLabel = ResolveStatusLabel(
          marcacao.StatusConsulta,
          null,
          null,
          null
        ),
      };
    }

    private static string? FormatDate(DateTime? value) =>
      value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static string? FormatTime(TimeSpan? value) =>
      value?.ToString(@"hh\:mm", CultureInfo.InvariantCulture);

    private static int? ToInt(StatusConsulta? status) =>
      status.HasValue ? (int)status.Value : null;

    private static string ResolveStatusLabel(
      StatusConsulta? status,
      bool? confirmado,
      bool? efetuado,
      bool? faltou
    )
    {
      if (status.HasValue)
      {
        return EnumDisplayHelper.GetDisplayName(status.Value);
      }

      if (efetuado == true)
      {
        return EnumDisplayHelper.GetDisplayName(StatusConsulta.Concluida);
      }

      if (faltou == true)
      {
        return EnumDisplayHelper.GetDisplayName(StatusConsulta.Faltou);
      }

      return confirmado == true ? "Presente" : "Pendente";
    }

    /// <summary>
    /// Preenche o número de utente com <c>Utente.NumeroUtente</c> via projeção sobre a entidade
    /// <see cref="Utente"/> (mesma origem que a listagem paginada de utentes).
    /// </summary>
    private async Task HydrateConsultaUtenteNumerosAsync(IReadOnlyCollection<ConsultaTableDTO> rows)
    {
      List<Guid> ids = rows
        .Where(r => r.UtenteId.HasValue)
        .Select(r => r.UtenteId!.Value)
        .Distinct()
        .ToList();
      if (ids.Count == 0)
      {
        return;
      }

      UtenteNumerosByIdsSpecification spec = new(ids);
      List<UtenteNumeroLookupDTO> lookups = (
        await _repository.GetListAsync<Utente, UtenteNumeroLookupDTO, Guid>(spec)
      ).ToList();
      Dictionary<Guid, string?> dict = lookups.ToDictionary(x => x.Id, x => x.NumeroUtente);
      foreach (ConsultaTableDTO row in rows)
      {
        if (row.UtenteId.HasValue && dict.TryGetValue(row.UtenteId.Value, out string? numero))
        {
          row.UtenteNumero = numero;
        }
      }
    }
  }
}
