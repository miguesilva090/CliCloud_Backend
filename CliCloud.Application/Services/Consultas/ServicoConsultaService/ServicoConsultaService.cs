using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.Filters;
using CliCloud.Application.Services.Consultas.ServicoConsultaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService
{
  public class ServicoConsultaService : IServicoConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ServicoConsultaService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ServicoConsultaDTO>>> GetServicoConsultaAsync(string keyword = "")
    {
      var spec = new ServicoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoConsulta, ServicoConsultaDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ServicoConsultaLightDTO>>> GetServicoConsultaLightAsync(string keyword = "")
    {
      var spec = new ServicoConsultaSearchList(keyword);
      var list = await _repository.GetListAsync<ServicoConsulta, ServicoConsultaLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ServicoConsultaTableDTO>> GetServicoConsultaPaginatedAsync(ServicoConsultaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ServicoConsultaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<ServicoConsulta, ServicoConsultaTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ServicoConsultaTableDTO>>> GetAllServicoConsultaAsync(ServicoConsultaAllFilter? filter)
    {
      try
      {
        filter ??= new ServicoConsultaAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ServicoConsultaSearchTable(filters, order);
        var list = await _repository.GetListAsync<ServicoConsulta, ServicoConsultaTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ServicoConsultaTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<ServicoConsultaDTO>> GetServicoConsultaAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<ServicoConsulta, ServicoConsultaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ServicoConsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateServicoConsultaAsync(CreateServicoConsultaRequest request)
    {
      try
      {
        // Garantir que a coluna Linha é única por Consulta (ConsultaId, Linha)
        // Se não vier definida, calcular próxima linha disponível para esta consulta.
        int nextLinha = request.Linha;

        if (nextLinha <= 0 && !string.IsNullOrWhiteSpace(request.ConsultaId) && GSHelpers.BeValidGuid(request.ConsultaId))
        {
          var filters = new List<TableFilter>
          {
            new() { Id = "consultaid", Value = request.ConsultaId }
          };

          // Ordenar por Linha desc para obter a última linha existente
          var spec = new ServicoConsultaSearchTable(filters, "linha desc");
          var existentes = await _repository.GetListAsync<ServicoConsulta, Guid>(spec);
          int lastLinha = existentes.FirstOrDefault()?.Linha ?? 0;
          nextLinha = lastLinha + 1;
        }

        var entity = _mapper.Map<ServicoConsulta>(request);
        entity.Linha = nextLinha;

        var created = await _repository.CreateAsync<ServicoConsulta, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateServicoConsultaAsync(UpdateServicoConsultaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<ServicoConsulta, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("ServicoConsulta não encontrado.");

      _ = _mapper.Map(request, existing);
      try
      {
        var updated = await _repository.UpdateAsync<ServicoConsulta, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteServicoConsultaAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<ServicoConsulta, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoConsultaAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<ServicoConsulta, Guid>(id);
          if (e == null) { fail.Add($"ServicoConsulta {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<ServicoConsulta, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"ServicoConsulta {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }
  }
}

