using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Filters;
using CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService
{
  public class ServicoTratamentoService : IServicoTratamentoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ServicoTratamentoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ServicoTratamentoDTO>>> GetServicoTratamentoAsync(string keyword = "")
    {
      var spec = new ServicoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoTratamento, ServicoTratamentoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ServicoTratamentoLightDTO>>> GetServicoTratamentoLightAsync(string keyword = "")
    {
      var spec = new ServicoTratamentoSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoTratamento, ServicoTratamentoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ServicoTratamentoTableDTO>> GetServicoTratamentoPaginatedAsync(ServicoTratamentoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ServicoTratamentoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ServicoTratamento, ServicoTratamentoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ServicoTratamentoTableDTO>>> GetAllServicoTratamentoAsync(ServicoTratamentoAllFilter? filter)
    {
      try
      {
        filter ??= new ServicoTratamentoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ServicoTratamentoSearchTable(filters, order);
        var list = await _repository.GetListAsync<ServicoTratamento, ServicoTratamentoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ServicoTratamentoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<ServicoTratamentoDTO>> GetServicoTratamentoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<ServicoTratamento, ServicoTratamentoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ServicoTratamentoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateServicoTratamentoAsync(CreateServicoTratamentoRequest request)
    {
      var entity = _mapper.Map<ServicoTratamento>(request);
      try
      {
        var created = await _repository.CreateAsync<ServicoTratamento, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateServicoTratamentoAsync(UpdateServicoTratamentoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<ServicoTratamento, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("ServicoTratamento não encontrado.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<ServicoTratamento, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteServicoTratamentoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<ServicoTratamento, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoTratamentoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<ServicoTratamento, Guid>(id);
          if (e == null) { fail.Add($"ServicoTratamento {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<ServicoTratamento, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"ServicoTratamento {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

