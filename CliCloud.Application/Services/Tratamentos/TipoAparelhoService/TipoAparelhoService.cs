using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Filters;
using CliCloud.Application.Services.Tratamentos.TipoAparelhoService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.TipoAparelhoService
{
  public class TipoAparelhoService : ITipoAparelhoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public TipoAparelhoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<TipoAparelhoDTO>>> GetTipoAparelhoAsync(string keyword = "")
    {
      var spec = new TipoAparelhoSearchList(keyword);
      var list = await _repository.GetListAsync<TipoAparelho, TipoAparelhoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<TipoAparelhoLightDTO>>> GetTipoAparelhoLightAsync(string keyword = "")
    {
      var spec = new TipoAparelhoSearchList(keyword);
      var list = await _repository.GetListAsync<TipoAparelho, TipoAparelhoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<TipoAparelhoTableDTO>> GetTipoAparelhoPaginatedAsync(TipoAparelhoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new TipoAparelhoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<TipoAparelho, TipoAparelhoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<TipoAparelhoTableDTO>>> GetAllTipoAparelhoAsync(TipoAparelhoAllFilter filter)
    {
      try
      {
        filter ??= new TipoAparelhoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new TipoAparelhoSearchTable(filters, order);
        var list = await _repository.GetListAsync<TipoAparelho, TipoAparelhoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<TipoAparelhoTableDTO>>(ex.Message); }
    }

    public async Task<Response<TipoAparelhoDTO>> GetTipoAparelhoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<TipoAparelho, TipoAparelhoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<TipoAparelhoDTO>(ex.Message); }
    }

    public async Task<Response<TipoAparelhoDTO>> GetTipoAparelhoByDesignacaoAsync(string designacao)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(designacao))
          return ResponseFactory.Fail<TipoAparelhoDTO>("Designação não pode ser vazia.");
        var spec = new TipoAparelhoMatchDesignacao(designacao);
        var results = await _repository.GetListAsync<TipoAparelho, TipoAparelhoDTO, Guid>(spec);
        var item = results.FirstOrDefault();
        if (item == null)
          return ResponseFactory.Fail<TipoAparelhoDTO>("Não foi encontrado nenhum tipo de aparelho com a designação fornecida.");
        return ResponseFactory.Success(item);
      }
      catch (Exception ex) { return ResponseFactory.Fail<TipoAparelhoDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateTipoAparelhoAsync(CreateTipoAparelhoRequest request)
    {
      var spec = new TipoAparelhoMatchDesignacao(request.Designacao);
      if (await _repository.ExistsAsync<TipoAparelho, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Tipo de aparelho com esta designação já existe.");
      var entity = _mapper.Map(request, new TipoAparelho());
      try
      {
        var created = await _repository.CreateAsync<TipoAparelho, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateTipoAparelhoAsync(UpdateTipoAparelhoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<TipoAparelho, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Tipo de aparelho não encontrado.");
      if (existing.Designacao != request.Designacao)
      {
        var spec = new TipoAparelhoMatchDesignacao(request.Designacao);
        if (await _repository.ExistsAsync<TipoAparelho, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe um tipo de aparelho com esta designação.");
      }
      _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<TipoAparelho, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> DeleteTipoAparelhoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<TipoAparelho, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleTipoAparelhoAsync(IEnumerable<Guid> ids)
    {
      try
      {
        var idsList = ids.ToList();
        var successfullyDeletedIds = new List<Guid>();
        var failedDeletions = new List<string>();
        foreach (var id in idsList)
        {
          try
          {
            var entity = await _repository.GetByIdAsync<TipoAparelho, Guid>(id);
            if (entity == null)
            {
              failedDeletions.Add($"Tipo de aparelho com ID {id} não encontrado.");
              continue;
            }
            var deletedEntity = await _repository.RemoveByIdAsync<TipoAparelho, Guid>(id);
            if (deletedEntity != null)
            {
              _ = await _repository.SaveChangesAsync();
              successfullyDeletedIds.Add(id);
            }
            else
              failedDeletions.Add($"Falha ao eliminar tipo de aparelho com ID {id}.");
          }
          catch (Exception)
          {
            failedDeletions.Add($"Falha ao eliminar tipo de aparelho com ID {id}.");
            _repository.ClearChangeTracker();
          }
        }
        if (successfullyDeletedIds.Count == idsList.Count)
          return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
        if (successfullyDeletedIds.Count > 0)
          return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} tipos de aparelho.");
        return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
      }
    }
  }
}
