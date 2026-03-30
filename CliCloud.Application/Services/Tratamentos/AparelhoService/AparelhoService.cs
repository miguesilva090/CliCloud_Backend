using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AparelhoService.Filters;
using CliCloud.Application.Services.Tratamentos.AparelhoService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService
{
  public class AparelhoService : IAparelhoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public AparelhoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<AparelhoDTO>>> GetAparelhoAsync(string keyword = "")
    {
      var spec = new AparelhoSearchList(keyword);
      var list = await _repository.GetListAsync<Aparelho, AparelhoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<AparelhoLightDTO>>> GetAparelhoLightAsync(string keyword = "")
    {
      var spec = new AparelhoSearchList(keyword);
      var list = await _repository.GetListAsync<Aparelho, AparelhoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<AparelhoTableDTO>> GetAparelhoPaginatedAsync(AparelhoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new AparelhoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Aparelho, AparelhoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<AparelhoTableDTO>>> GetAllAparelhoAsync(AparelhoAllFilter filter)
    {
      try
      {
        filter ??= new AparelhoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new AparelhoSearchTable(filters, order);
        var list = await _repository.GetListAsync<Aparelho, AparelhoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<AparelhoTableDTO>>(ex.Message); }
    }

    public async Task<Response<AparelhoDTO>> GetAparelhoAsync(Guid id)
    {
      try
      {
        var spec = new AparelhoByIdWithIncludes(id);
        var dto = await _repository.GetByIdAsync<Aparelho, AparelhoDTO, Guid>(id, spec);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<AparelhoDTO>(ex.Message); }
    }

    public async Task<Response<AparelhoDTO>> GetAparelhoByCodigoSerieAsync(string codigoSerie)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(codigoSerie))
          return ResponseFactory.Fail<AparelhoDTO>("Código de série não pode ser vazio.");
        var spec = new AparelhoMatchCodigoSerie(codigoSerie);
        var results = await _repository.GetListAsync<Aparelho, AparelhoDTO, Guid>(spec);
        var item = results.FirstOrDefault();
        if (item == null)
          return ResponseFactory.Fail<AparelhoDTO>("Não foi encontrado nenhum aparelho com o código de série fornecido.");
        return ResponseFactory.Success(item);
      }
      catch (Exception ex) { return ResponseFactory.Fail<AparelhoDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateAparelhoAsync(CreateAparelhoRequest request)
    {
      if (!Guid.TryParse(request.TipoAparelhoId, out var tipoId))
        return ResponseFactory.Fail<Guid>("TipoAparelhoId inválido.");
      Guid? modeloId = null;
      if (!string.IsNullOrWhiteSpace(request.ModeloAparelhoId) && Guid.TryParse(request.ModeloAparelhoId, out var parsedModelo))
        modeloId = parsedModelo;
      if (!string.IsNullOrWhiteSpace(request.CodigoSerie))
      {
        var specExists = new AparelhoMatchCodigoSerie(request.CodigoSerie);
        if (await _repository.ExistsAsync<Aparelho, Guid>(specExists))
          return ResponseFactory.Fail<Guid>("Já existe um aparelho com este código de série.");
      }
      var entity = _mapper.Map(request, new Aparelho());
      entity.TipoAparelhoId = tipoId;
      entity.ModeloAparelhoId = modeloId;
      try
      {
        var created = await _repository.CreateAsync<Aparelho, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateAparelhoAsync(UpdateAparelhoRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Aparelho, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Aparelho não encontrado.");
      if (!Guid.TryParse(request.TipoAparelhoId, out var tipoId))
        return ResponseFactory.Fail<Guid>("TipoAparelhoId inválido.");
      Guid? modeloId = null;
      if (!string.IsNullOrWhiteSpace(request.ModeloAparelhoId) && Guid.TryParse(request.ModeloAparelhoId, out var parsedModelo))
        modeloId = parsedModelo;
      if (!string.IsNullOrWhiteSpace(request.CodigoSerie) && existing.CodigoSerie != request.CodigoSerie)
      {
        var specExists = new AparelhoMatchCodigoSerie(request.CodigoSerie, excludeId: id);
        if (await _repository.ExistsAsync<Aparelho, Guid>(specExists))
          return ResponseFactory.Fail<Guid>("Já existe outro aparelho com este código de série.");
      }
      _mapper.Map(request, existing);
      existing.TipoAparelhoId = tipoId;
      existing.ModeloAparelhoId = modeloId;
      try
      {
        var updated = await _repository.UpdateAsync<Aparelho, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> DeleteAparelhoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Aparelho, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleAparelhoAsync(IEnumerable<Guid> ids)
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
            var entity = await _repository.GetByIdAsync<Aparelho, Guid>(id);
            if (entity == null)
            {
              failedDeletions.Add($"Aparelho com ID {id} não encontrado.");
              continue;
            }
            var deletedEntity = await _repository.RemoveByIdAsync<Aparelho, Guid>(id);
            if (deletedEntity != null)
            {
              _ = await _repository.SaveChangesAsync();
              successfullyDeletedIds.Add(id);
            }
            else
              failedDeletions.Add($"Falha ao eliminar aparelho com ID {id}.");
          }
          catch (Exception)
          {
            failedDeletions.Add($"Falha ao eliminar aparelho com ID {id}.");
            _repository.ClearChangeTracker();
          }
        }
        if (successfullyDeletedIds.Count == idsList.Count)
          return ResponseFactory.Success<IEnumerable<Guid>>(successfullyDeletedIds);
        if (successfullyDeletedIds.Count > 0)
          return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(successfullyDeletedIds, $"Eliminados com sucesso {successfullyDeletedIds.Count} de {idsList.Count} aparelhos.");
        return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", failedDeletions));
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<Guid>>(ex.Message);
      }
    }
  }
}
