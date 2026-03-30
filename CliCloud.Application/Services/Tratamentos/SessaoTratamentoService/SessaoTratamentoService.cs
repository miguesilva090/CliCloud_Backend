using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService
{
  public class SessaoTratamentoService : ISessaoTratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public SessaoTratamentoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<SessaoTratamentoDTO>>> GetSessaoTratamentoAsync(string keyword = "")
    {
      var spec = new SessaoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<SessaoTratamentoLightDTO>>> GetSessaoTratamentoLightAsync(string keyword = "")
    {
      var spec = new SessaoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<SessaoTratamentoTableDTO>> GetSessaoTratamentoPaginatedAsync(SessaoTratamentoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new SessaoTratamentoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<SessaoTratamento, SessaoTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<SessaoTratamentoTableDTO>>> GetAllSessaoTratamentoAsync(SessaoTratamentoAllFilter? filter)
    {
      try
      {
        filter ??= new SessaoTratamentoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new SessaoTratamentoSearchTable(filters, order);
        var list = await _repository.GetListAsync<SessaoTratamento, SessaoTratamentoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<SessaoTratamentoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<SessaoTratamentoDTO>> GetSessaoTratamentoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<SessaoTratamento, SessaoTratamentoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<SessaoTratamentoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateSessaoTratamentoAsync(CreateSessaoTratamentoRequest request)
    {
      var entity = _mapper.Map<SessaoTratamento>(request);
      try
      {
        var created = await _repository.CreateAsync<SessaoTratamento, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateSessaoTratamentoAsync(UpdateSessaoTratamentoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("SessaoTratamento não encontrada.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<SessaoTratamento, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteSessaoTratamentoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSessaoTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
          if (e == null) { fail.Add($"SessaoTratamento {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<SessaoTratamento, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"SessaoTratamento {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

