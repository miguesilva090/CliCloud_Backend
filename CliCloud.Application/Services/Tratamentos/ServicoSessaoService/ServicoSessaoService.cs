using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Filters;
using CliCloud.Application.Services.Tratamentos.ServicoSessaoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService
{
  public class ServicoSessaoService : IServicoSessaoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ServicoSessaoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ServicoSessaoDTO>>> GetServicoSessaoAsync(string keyword = "")
    {
      var spec = new ServicoSessaoSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoSessao, ServicoSessaoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ServicoSessaoLightDTO>>> GetServicoSessaoLightAsync(string keyword = "")
    {
      var spec = new ServicoSessaoSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoSessao, ServicoSessaoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ServicoSessaoTableDTO>> GetServicoSessaoPaginatedAsync(ServicoSessaoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ServicoSessaoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ServicoSessao, ServicoSessaoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ServicoSessaoTableDTO>>> GetAllServicoSessaoAsync(ServicoSessaoAllFilter? filter)
    {
      try
      {
        filter ??= new ServicoSessaoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ServicoSessaoSearchTable(filters, order);
        var list = await _repository.GetListAsync<ServicoSessao, ServicoSessaoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ServicoSessaoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<ServicoSessaoDTO>> GetServicoSessaoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<ServicoSessao, ServicoSessaoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ServicoSessaoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateServicoSessaoAsync(CreateServicoSessaoRequest request)
    {
      var entity = _mapper.Map<ServicoSessao>(request);
      try
      {
        var created = await _repository.CreateAsync<ServicoSessao, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateServicoSessaoAsync(UpdateServicoSessaoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<ServicoSessao, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("ServicoSessao não encontrado.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<ServicoSessao, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteServicoSessaoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<ServicoSessao, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoSessaoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<ServicoSessao, Guid>(id);
          if (e == null) { fail.Add($"ServicoSessao {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<ServicoSessao, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"ServicoSessao {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

