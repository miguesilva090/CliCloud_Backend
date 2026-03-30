using System.Linq;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Enums;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.Filters;
using CliCloud.Application.Services.Seguradoras.SeguradoraService.Specifications;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService
{
  public class SeguradoraService : ISeguradoraService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public SeguradoraService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<SeguradoraDTO>>> GetSeguradoraAsync(string keyword = "")
    {
      var spec = new SeguradoraSearchList(keyword);
      var list = await _repository.GetListAsync<Seguradora, SeguradoraDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<SeguradoraLightDTO>>> GetSeguradoraLightAsync(string keyword = "")
    {
      var spec = new SeguradoraSearchList(keyword);
      var list = await _repository.GetListAsync<Seguradora, SeguradoraLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<SeguradoraTableDTO>> GetSeguradoraPaginatedAsync(SeguradoraTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new SeguradoraSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Seguradora, SeguradoraTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<SeguradoraTableDTO>>> GetAllSeguradoraAsync(SeguradoraAllFilter filter)
    {
      try
      {
        filter ??= new SeguradoraAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new SeguradoraSearchTable(filters, order);
        var list = await _repository.GetListAsync<Seguradora, SeguradoraTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<SeguradoraTableDTO>>(ex.Message); }
    }

    public async Task<Response<SeguradoraDTO>> GetSeguradoraAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Seguradora, SeguradoraDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<SeguradoraDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreateSeguradoraAsync(CreateSeguradoraRequest request)
    {
      var spec = new SeguradoraMatchNome(request.Nome);
      if (await _repository.ExistsAsync<Seguradora, Guid>(spec))
        return ResponseFactory.Fail<Guid>("Seguradora com este nome já existe.");
      var entity = _mapper.Map<Seguradora>(request);
      entity.TipoEntidade = EntidadeTipo.Seguradora;
      if (!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out var bancoId))
        entity.BancoId = bancoId;
      try
      {
        var created = await _repository.CreateAsync<Seguradora, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdateSeguradoraAsync(UpdateSeguradoraRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<Seguradora, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Seguradora não encontrada.");
      if (existing.Nome != request.Nome)
      {
        var spec = new SeguradoraMatchNome(request.Nome);
        if (await _repository.ExistsAsync<Seguradora, Guid>(spec))
          return ResponseFactory.Fail<Guid>("Já existe uma seguradora com este nome.");
      }
      _mapper.Map(request, existing);
      existing.TipoEntidade = EntidadeTipo.Seguradora;
      if (!string.IsNullOrWhiteSpace(request.BancoId) && Guid.TryParse(request.BancoId, out var bancoId))
        existing.BancoId = bancoId;
      else
        existing.BancoId = null;
      try
      {
        var updated = await _repository.UpdateAsync<Seguradora, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> DeleteSeguradoraAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Seguradora, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleSeguradoraAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Seguradora, Guid>(id);
          if (e == null) { fail.Add($"Seguradora {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Seguradora, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch { fail.Add($"Seguradora {id}."); _repository.ClearChangeTracker(); }
      }
      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}
