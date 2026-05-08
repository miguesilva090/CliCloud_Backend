using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Utility.TipoCartaService.DTOs;
using CliCloud.Application.Services.Utility.TipoCartaService.Filters;
using CliCloud.Application.Services.Utility.TipoCartaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Common;

namespace CliCloud.Application.Services.Utility.TipoCartaService
{
  public class TipoCartaService(IRepositoryAsync repository, IMapper mapper) : ITipoCartaService
  {
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<IEnumerable<TipoCartaDTO>>> GetAllAsync(string keyword = "")
    {
      var spec = new TipoCartaSearchList(keyword);
      var list = await _repository.GetListAsync<TipoCarta, TipoCartaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<TipoCartaTableDTO>> GetPaginatedAsync(TipoCartaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0)
      {
        filter.PageNumber = 1;
      }

      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
      var spec = new TipoCartaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<TipoCarta, TipoCartaTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );
    }

    public async Task<Response<TipoCartaDTO>> GetByIdAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<TipoCarta, TipoCartaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<TipoCartaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAsync(CreateTipoCartaRequest request)
    {
      var duplicateSpec = new TipoCartaMatchDescricao(request.Descricao ?? string.Empty);
      if (await _repository.ExistsAsync<TipoCarta, Guid>(duplicateSpec))
      {
        return ResponseFactory.Fail<Guid>("Já existe um tipo de carta com esta descrição.");
      }

      var entity = _mapper.Map<TipoCarta>(request);
      entity.Id = Guid.NewGuid();

      try
      {
        var created = await _repository.CreateAsync<TipoCarta, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateTipoCartaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<TipoCarta, Guid>(id);
      if (existing == null)
      {
        return ResponseFactory.Fail<Guid>("Tipo de carta não encontrado.");
      }

      var duplicateSpec = new TipoCartaMatchDescricao(request.Descricao ?? string.Empty, id);
      if (await _repository.ExistsAsync<TipoCarta, Guid>(duplicateSpec))
      {
        return ResponseFactory.Fail<Guid>("Já existe um tipo de carta com esta descrição.");
      }

      _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<TipoCarta, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteAsync(Guid id)
    {
      try
      {
        var removed = await _repository.RemoveByIdAsync<TipoCarta, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(removed.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var existing = await _repository.GetByIdAsync<TipoCarta, Guid>(id);
          if (existing == null)
          {
            fail.Add($"Tipo de carta {id} não encontrado.");
            continue;
          }

          var removed = await _repository.RemoveByIdAsync<TipoCarta, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Tipo de carta {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count)
      {
        return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      }

      if (ok.Count > 0)
      {
        return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      }

      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}
