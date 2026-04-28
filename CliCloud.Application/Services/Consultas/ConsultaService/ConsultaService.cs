using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Consultas.ConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ConsultaService.Filters;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
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
      var spec = new ConsultaSearchTable(filter.Filters ?? [], order);
      PaginatedResponse<ConsultaTableDTO> result = await _repository.GetPaginatedResultsAsync<
        Consulta,
        ConsultaTableDTO,
        Guid
      >(filter.PageNumber, filter.PageSize, spec);
      await HydrateConsultaUtenteNumerosAsync(result.Data);
      return result;
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
      try
      {
        var marcacao = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(marcacaoId);
        if (marcacao == null)
        {
          return ResponseFactory.Fail<Guid>("Marcação de consulta não encontrada.");
        }

        if (marcacao.ConsultaId != null)
        {
          return ResponseFactory.Fail<Guid>("Já existe uma consulta associada a esta marcação.");
        }

        Guid? medicoId = marcacao.MedicoId;

        if (medicoId == null)
        {
          string? userIdStr = _currentTenantUserService.UserId;
          if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out Guid userId))
          {
            var medicoRes = await _medicoService.GetMedicoByIdUtilizadorAsync(userId);
            if (medicoRes.Status == ResponseStatus.Success && medicoRes.Data != null)
            {
              medicoId = medicoRes.Data.Id;
              marcacao.MedicoId = medicoId;
            }
          }
        }

        var consulta = new Consulta
        {
          UtenteId = marcacao.UtenteId,
          MedicoId = medicoId,
          EspecialidadeId = marcacao.EspecialidadeId,
          TecnicoId = marcacao.TecnicoId,
          SalaId = marcacao.SalaId,
          MedicoExternoId = marcacao.MedicoExternoId,
          Data = marcacao.Data,
          HoraInicio = marcacao.HoraMarcacao,
          StatusConsulta = StatusConsulta.EmAtendimento,
          ConsultaMarcacaoId = marcacao.Id,
          TipoConsultaId = marcacao.TipoConsultaId,
          Sinistrado = 0,
        };

        var created = await _repository.CreateAsync<Consulta, Guid>(consulta);

        marcacao.ConsultaId = created.Id;
        marcacao.StatusConsulta = StatusConsulta.EmAtendimento;
        _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
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

        consulta.StatusConsulta = StatusConsulta.Concluida;
        consulta.HoraFim = horaFim;
        _ = await _repository.UpdateAsync<Consulta, Guid>(consulta);

        var marcacoes = await _repository.GetListAsync<ConsultaMarcacao, Guid>();
        foreach (var marcacao in marcacoes.Where(x => x.ConsultaId == id && x.DeletedOn == null))
        {
          marcacao.StatusConsulta = StatusConsulta.Concluida;
          _ = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(marcacao);
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
