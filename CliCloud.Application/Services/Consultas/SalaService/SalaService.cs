using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.SalaService.DTOs;
using CliCloud.Application.Services.Consultas.SalaService.Filters;
using CliCloud.Application.Services.Consultas.SalaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.SalaService
{
  public class SalaService(IRepositoryAsync repository, IMapper mapper) : ISalaService
  {
    private readonly IRepositoryAsync _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<IEnumerable<SalaDTO>>> GetAllAsync(string keyword = "")
    {
      var spec = new SalaSearchList(keyword);
      var list = await _repository.GetListAsync<Sala, SalaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<SalaTableDTO>> GetPaginatedAsync(SalaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0)
      {
        filter.PageNumber = 1;
      }

      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
      var spec = new SalaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Sala, SalaTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );
    }

    public async Task<Response<SalaDTO>> GetByIdAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Sala, SalaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<SalaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAsync(CreateSalaRequest request)
    {
      var duplicateSpec = new SalaMatchNomeNumero(request.Nome ?? string.Empty, request.NumeroSala);
      if (await _repository.ExistsAsync<Sala, Guid>(duplicateSpec))
      {
        return ResponseFactory.Fail<Guid>("Já existe uma sala com este nome ou número.");
      }

      var entity = _mapper.Map<Sala>(request);
      entity.Id = Guid.NewGuid();

      try
      {
        var created = await _repository.CreateAsync<Sala, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateSalaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Sala, Guid>(id);
      if (existing == null)
      {
        return ResponseFactory.Fail<Guid>("Sala não encontrada.");
      }

      var duplicateSpec = new SalaMatchNomeNumero(request.Nome ?? string.Empty, request.NumeroSala, id);
      if (await _repository.ExistsAsync<Sala, Guid>(duplicateSpec))
      {
        return ResponseFactory.Fail<Guid>("Já existe uma sala com este nome ou número.");
      }

      _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<Sala, Guid>(existing);
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
        var removed = await _repository.RemoveByIdAsync<Sala, Guid>(id);
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
          var existing = await _repository.GetByIdAsync<Sala, Guid>(id);
          if (existing == null)
          {
            fail.Add($"Sala {id} não encontrada.");
            continue;
          }

          var removed = await _repository.RemoveByIdAsync<Sala, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Sala {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count)
      {
        return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      }

      if (ok.Count > 0)
      {
        return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      }

      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}
