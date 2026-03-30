using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Filters;
using CliCloud.Application.Services.Consultas.MarcacaoConsultaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService
{
  public class MarcacaoConsultaService : IMarcacaoConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public MarcacaoConsultaService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaDTO>>> GetMarcacaoConsultaAsync(string keyword = "")
    {
      var spec = new MarcacaoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaLightDTO>>> GetMarcacaoConsultaLightAsync(string keyword = "")
    {
      var spec = new MarcacaoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<MarcacaoConsultaTableDTO>> GetMarcacaoConsultaPaginatedAsync(MarcacaoConsultaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new MarcacaoConsultaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ConsultaMarcacao, MarcacaoConsultaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<MarcacaoConsultaTableDTO>>> GetAllMarcacaoConsultaAsync(MarcacaoConsultaAllFilter? filter)
    {
      try
      {
        filter ??= new MarcacaoConsultaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new MarcacaoConsultaSearchTable(filters, order);
        var list = await _repository.GetListAsync<ConsultaMarcacao, MarcacaoConsultaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<MarcacaoConsultaTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<MarcacaoConsultaDTO>> GetMarcacaoConsultaAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<ConsultaMarcacao, MarcacaoConsultaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<MarcacaoConsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateMarcacaoConsultaAsync(CreateMarcacaoConsultaRequest request)
    {
      var entity = _mapper.Map<ConsultaMarcacao>(request);
      try
      {
        var created = await _repository.CreateAsync<ConsultaMarcacao, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateMarcacaoConsultaAsync(UpdateMarcacaoConsultaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("MarcacaoConsulta não encontrada.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<ConsultaMarcacao, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteMarcacaoConsultaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<ConsultaMarcacao, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleMarcacaoConsultaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<ConsultaMarcacao, Guid>(id);
          if (e == null) { fail.Add($"MarcacaoConsulta {id} não encontrada."); continue; }
          var removed = await _repository.RemoveByIdAsync<ConsultaMarcacao, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"MarcacaoConsulta {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminadas {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

