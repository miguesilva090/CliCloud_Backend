using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;
using CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs;
using CliCloud.Application.Services.Tratamentos.PatologiaService.Filters;
using CliCloud.Application.Services.Tratamentos.PatologiaService.Specifications;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService
{
  public class PatologiaService : IPatologiaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public PatologiaService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<PatologiaDTO>>> GetPatologiaAsync(string keyword = "")
    {
      var spec = new PatologiaSearchList(keyword);
      var list = await _repository.GetListAsync<Patologia, PatologiaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<PatologiaLightDTO>>> GetPatologiaLightAsync(string keyword = "")
    {
      var spec = new PatologiaSearchList(keyword);
      var list = await _repository.GetListAsync<Patologia, PatologiaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<PatologiaTableDTO>> GetPatologiaPaginatedAsync(PatologiaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new PatologiaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Patologia, PatologiaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<PatologiaTableDTO>>> GetAllPatologiaAsync(PatologiaAllFilter filter)
    {
      try
      {
        filter ??= new PatologiaAllFilter();
        var order = PatologiaAllFilter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new PatologiaSearchTable(filters, order);
        var list = await _repository.GetListAsync<Patologia, PatologiaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex) { return ResponseFactory.Fail<IEnumerable<PatologiaTableDTO>>(ex.Message); }
    }

    public async Task<Response<PatologiaDTO>> GetPatologiaAsync(Guid id)
    {
      try
      {
        var spec = new PatologiaByIdWithIncludes(id);
        var dto = await _repository.GetByIdAsync<Patologia, PatologiaDTO, Guid>(id, spec);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex) { return ResponseFactory.Fail<PatologiaDTO>(ex.Message); }
    }

    public async Task<Response<Guid>> CreatePatologiaAsync(CreatePatologiaRequest request)
    {
      var entity = _mapper.Map<Patologia>(request);
      entity.Id = Guid.NewGuid();
      try
      {
        var created = await _repository.CreateAsync<Patologia, Guid>(entity);
        if (request.PatologiaServicos != null)
        {
          var ordem = 0;
          foreach (var req in request.PatologiaServicos)
          {
            var ps = _mapper.Map<PatologiaServico>(req);
            ps.Id = Guid.NewGuid();
            ps.PatologiaId = created.Id;
            ps.Ordem = req.Ordem;
            if (ps.Ordem == 0) ps.Ordem = ordem++;
            _ = await _repository.CreateAsync<PatologiaServico, Guid>(ps);
          }
        }
        if (request.DoencaIds != null)
        {
          foreach (var doencaId in request.DoencaIds)
            created.PatologiaDoencas.Add(new PatologiaDoenca { PatologiaId = created.Id, DoencaId = doencaId });
        }
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> UpdatePatologiaAsync(UpdatePatologiaRequest request, Guid id)
    {
      var spec = new PatologiaByIdWithIncludes(id);
      var existing = await _repository.GetByIdAsync<Patologia, Guid>(id, spec);
      if (existing == null) return ResponseFactory.Fail<Guid>("Patologia não encontrada.");

      _mapper.Map(request, existing);

      foreach (var ps in existing.PatologiaServicos.ToList())
        await _repository.RemoveAsync<PatologiaServico, Guid>(ps);

      existing.PatologiaDoencas.Clear();

      if (request.DoencaIds != null)
      {
        foreach (var doencaId in request.DoencaIds)
          existing.PatologiaDoencas.Add(new PatologiaDoenca { PatologiaId = id, DoencaId = doencaId });
      }

      if (request.PatologiaServicos != null)
      {
        var ordem = 0;
        foreach (var req in request.PatologiaServicos)
        {
          var ps = _mapper.Map<PatologiaServico>(req);
          ps.Id = Guid.NewGuid();
          ps.PatologiaId = id;
          ps.Ordem = req.Ordem;
          if (ps.Ordem == 0) ps.Ordem = ordem++;
          _ = await _repository.CreateAsync<PatologiaServico, Guid>(ps);
        }
      }

      try
      {
        _ = await _repository.UpdateAsync<Patologia, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(existing.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<Guid>> DeletePatologiaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Patologia, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex) { return ResponseFactory.Fail<Guid>(ex.Message); }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultiplePatologiaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();
      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Patologia, Guid>(id);
          if (e == null) { fail.Add($"Patologia {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<Patologia, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch { fail.Add($"Patologia {id}."); _repository.ClearChangeTracker(); }
      }
      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}
