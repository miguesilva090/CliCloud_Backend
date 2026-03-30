using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.TipoServicoService.DTOs;
using CliCloud.Application.Services.Servicos.TipoServicoService.Filters;
using CliCloud.Application.Services.Servicos.TipoServicoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Servicos.TipoServicoService
{
  public class TipoServicoService : ITipoServicoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public TipoServicoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<TipoServicoDTO>>> GetTipoServicoAsync(string keyword = "")
    {
      var spec = new TipoServicoSearchList(keyword);
      var list = await _repository.GetListAsync<TipoServico, TipoServicoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<TipoServicoLightDTO>>> GetTipoServicoLightAsync(string keyword = "")
    {
      var spec = new TipoServicoSearchList(keyword);
      var list = await _repository.GetListAsync<TipoServico, TipoServicoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<TipoServicoTableDTO>> GetTipoServicoPaginatedAsync(TipoServicoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new TipoServicoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<TipoServico, TipoServicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<TipoServicoTableDTO>>> GetAllTipoServicoAsync(TipoServicoAllFilter? filter)
    {
      try
      {
        filter ??= new TipoServicoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new TipoServicoSearchTable(filters, order);
        var list = await _repository.GetListAsync<TipoServico, TipoServicoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<TipoServicoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<TipoServicoDTO>> GetTipoServicoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<TipoServico, TipoServicoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TipoServicoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateTipoServicoAsync(CreateTipoServicoRequest request)
    {
      var spec = new TipoServicoMatchNome(request.Descricao);
      if (await _repository.ExistsAsync<TipoServico, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Já existe um tipo de serviço com este nome.");

      var entity = _mapper.Map<TipoServico>(request);
      try
      {
        var created = await _repository.CreateAsync<TipoServico, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateTipoServicoAsync(UpdateTipoServicoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<TipoServico, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("TipoServico não encontrado.");

      if (!string.Equals(existing.Descricao, request.Descricao, StringComparison.Ordinal))
      {
        var spec = new TipoServicoMatchNome(request.Descricao);
        if (await _repository.ExistsAsync<TipoServico, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe um tipo de serviço com este nome.");
      }

      _ = _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<TipoServico, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteTipoServicoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<TipoServico, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoServicoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<TipoServico, Guid>(id);
          if (e == null) { fail.Add($"TipoServico {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<TipoServico, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"TipoServico {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

