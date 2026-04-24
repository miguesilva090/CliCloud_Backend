using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Notificacoes;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Filters;
using CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.Specifications;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService;

public class NotificacaoTipoService : INotificacaoTipoService
{
  private readonly IRepositoryAsync _repository;
  private readonly IMapper _mapper;

  public NotificacaoTipoService(IRepositoryAsync repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Response<IEnumerable<NotificacaoTipoDTO>>> GetNotificacaoTipoAsync(string keyword = "")
  {
    NotificacaoTipoSearchList specification = new(keyword);
    IEnumerable<NotificacaoTipoDTO> list =
      await _repository.GetListAsync<NotificacaoTipo, NotificacaoTipoDTO, Guid>(specification);
    return ResponseFactory.Success<IEnumerable<NotificacaoTipoDTO>>(list);
  }

  public async Task<Response<IEnumerable<NotificacaoTipoLightDTO>>> GetNotificacaoTipoLightAsync(string keyword = "")
  {
    NotificacaoTipoSearchList specification = new(keyword);
    IEnumerable<NotificacaoTipoLightDTO> list =
      await _repository.GetListAsync<NotificacaoTipo, NotificacaoTipoLightDTO, Guid>(specification);
    return ResponseFactory.Success<IEnumerable<NotificacaoTipoLightDTO>>(list);
  }

  public async Task<PaginatedResponse<NotificacaoTipoTableDTO>> GetNotificacaoTipoPaginatedAsync(
    NotificacaoTipoTableFilter filter)
  {
    if (filter.Filters is { Count: > 0 })
      filter.PageNumber = 1;

    string dynamicOrder = filter.Sorting is { Count: > 0 } ? GSHelpers.GenerateOrderByString(filter) : "";
    NotificacaoTipoSearchTable specification = new(filter.Filters ?? [], dynamicOrder);
    return await _repository.GetPaginatedResultsAsync<NotificacaoTipo, NotificacaoTipoTableDTO, Guid>(
      filter.PageNumber,
      filter.PageSize,
      specification);
  }

  public async Task<Response<IEnumerable<NotificacaoTipoTableDTO>>> GetAllNotificacaoTipoAsync(NotificacaoTipoAllFilter filter)
  {
    try
    {
      filter ??= new NotificacaoTipoAllFilter();
      string dynamicOrder = filter.GetOrderByString();
      List<TableFilter> tableFilters = filter.Filters ?? [];
      NotificacaoTipoSearchTable specification = new(tableFilters, dynamicOrder);
      IEnumerable<NotificacaoTipoTableDTO> list =
        await _repository.GetListAsync<NotificacaoTipo, NotificacaoTipoTableDTO, Guid>(specification);
      return ResponseFactory.Success<IEnumerable<NotificacaoTipoTableDTO>>(list);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<IEnumerable<NotificacaoTipoTableDTO>>(ex.Message);
    }
  }

  public async Task<Response<NotificacaoTipoDTO>> GetNotificacaoTipoAsync(Guid id)
  {
    try
    {
      NotificacaoTipoDTO dto = await _repository.GetByIdAsync<NotificacaoTipo, NotificacaoTipoDTO, Guid>(id);
      return ResponseFactory.Success(dto);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<NotificacaoTipoDTO>(ex.Message);
    }
  }

  public async Task<Response<Guid>> CreateNotificacaoTipoAsync(CreateNotificacaoTipoRequest request)
  {
    NotificacaoTipoMatchDesignacaoTipo specification = new(request.DesignacaoTipo.Trim());
    bool exists = await _repository.ExistsAsync<NotificacaoTipo, Guid>(specification);
    if (exists)
      return ResponseFactory.Fail<Guid>("Já existe um tipo com esta designação.");

    NotificacaoTipo entity = _mapper.Map(request, new NotificacaoTipo());
    try
    {
      NotificacaoTipo response = await _repository.CreateAsync<NotificacaoTipo, Guid>(entity);
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(response.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  public async Task<Response<Guid>> UpdateNotificacaoTipoAsync(UpdateNotificacaoTipoRequest request, Guid id)
  {
    NotificacaoTipo entityInDb = await _repository.GetByIdAsync<NotificacaoTipo, Guid>(id);
    if (entityInDb.ReservadoSistema)
      return ResponseFactory.Fail<Guid>("Impossível modificar este registro.");

    string current = entityInDb.DesignacaoTipo?.Trim() ?? string.Empty;
    string requested = request.DesignacaoTipo?.Trim() ?? string.Empty;

    if (!string.Equals(current, requested, StringComparison.OrdinalIgnoreCase))
    {
      NotificacaoTipoMatchDesignacaoTipo specification = new(requested);
      bool exists = await _repository.ExistsAsync<NotificacaoTipo, Guid>(specification);
      if (exists)
        return ResponseFactory.Fail<Guid>("Já existe um tipo com esta designação.");
    }

    _ = _mapper.Map(request, entityInDb);
    try
    {
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(entityInDb.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  public async Task<Response<Guid>> DeleteNotificacaoTipoAsync(Guid id)
  {
    try
    {
      NotificacaoTipo entity = await _repository.GetByIdAsync<NotificacaoTipo, Guid>(id);
      if (entity.ReservadoSistema)
        return ResponseFactory.Fail<Guid>("Não é possível eliminar um tipo reservado ao sistema.");

      NotificacaoTipo removed = await _repository.RemoveByIdAsync<NotificacaoTipo, Guid>(id);
      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(removed.Id);
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<Guid>(ex.Message);
    }
  }

  public async Task<Response<IEnumerable<Guid>>> DeleteMultipleNotificacaoTipoAsync(IEnumerable<Guid> ids)
  {
    try
    {
      List<Guid> idsList = ids.ToList();
      List<Guid> ok = [];
      List<string> failed = [];

      foreach (Guid id in idsList)
      {
        try
        {
          Response<Guid> one = await DeleteNotificacaoTipoAsync(id);
          if (one.Status == ResponseStatus.Success && one.Data != Guid.Empty)
            ok.Add(id);
          else
            failed.Add(id.ToString());
        }
        catch (Exception)
        {
          failed.Add(id.ToString());
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == idsList.Count)
        return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0)
        return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {idsList.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failed));
    }
    catch (Exception ex)
    {
      return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
    }
  }
}
