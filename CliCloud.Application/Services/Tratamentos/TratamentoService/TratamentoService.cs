using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.TratamentoService.Specifications;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService
{
  public class TratamentoService : ITratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public TratamentoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<TratamentoDTO>>> GetTratamentoAsync(string keyword = "")
    {
      var spec = new TratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<Tratamento, TratamentoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<TratamentoLightDTO>>> GetTratamentoLightAsync(string keyword = "")
    {
      var spec = new TratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<Tratamento, TratamentoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<TratamentoTableDTO>> GetTratamentoPaginatedAsync(TratamentoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new TratamentoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Tratamento, TratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<TratamentoTableDTO>>> GetAllTratamentoAsync(TratamentoAllFilter? filter)
    {
      try
      {
        filter ??= new TratamentoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new TratamentoSearchTable(filters, order);
        var list = await _repository.GetListAsync<Tratamento, TratamentoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<TratamentoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<TratamentoDTO>> GetTratamentoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Tratamento, TratamentoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TratamentoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateTratamentoAsync(CreateTratamentoRequest request)
    {
      var entity = _mapper.Map<Tratamento>(request);
      try
      {
        var created = await _repository.CreateAsync<Tratamento, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateTratamentoAsync(UpdateTratamentoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Tratamento, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<Tratamento, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteTratamentoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Tratamento, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Tratamento, Guid>(id);
          if (e == null) { fail.Add($"Tratamento {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<Tratamento, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Tratamento {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    /// <summary>
    /// Atualiza o estado de "alta" de um tratamento.
    /// No modelo novo vamos usar DataFim como indicador de alta:
    /// - alta = true  => DataFim = DateTime.UtcNow (se ainda não tiver)
    /// - alta = false => DataFim = null
    /// </summary>
    public async Task<Response<Guid>> UpdateTratamentoAltaAsync(Guid id, bool alta)
    {
      var existing = await _repository.GetByIdAsync<Tratamento, Guid>(id);
      if (existing == null)
      {
        return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");
      }

      if (alta)
      {
        existing.DataFim ??= DateTime.UtcNow;
      }
      else
      {
        existing.DataFim = null;
      }

      // Sincronizar o estado de Alta também na última Evolução de Tratamento, quando existir.
      try
      {
        var evolucaoSpec = new EvolucaoTratamentoByTratamentoIdUltima(existing.Id);
        IEnumerable<EvolucaoTratamento> evolucoes =
          await _repository.GetListAsync<EvolucaoTratamento, Guid>(evolucaoSpec);

        EvolucaoTratamento? ultimaEvolucao = evolucoes.FirstOrDefault();
        if (ultimaEvolucao != null)
        {
          if (alta)
          {
            ultimaEvolucao.DataAlta ??= DateTime.UtcNow;
          }
          else
          {
            ultimaEvolucao.DataAlta = null;
          }

          _ = await _repository.UpdateAsync<EvolucaoTratamento, Guid>(ultimaEvolucao);
        }
      }
      catch
      {
        // Se falhar a sincronização da evolução, não impedimos a atualização do tratamento.
      }

      try
      {
        var updated = await _repository.UpdateAsync<Tratamento, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}

