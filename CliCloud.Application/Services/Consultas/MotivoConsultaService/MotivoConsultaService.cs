using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.Filters;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Consultas.MotivoConsultaService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService
{
  public class MotivoConsultaService : IMotivoConsultaService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public MotivoConsultaService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<MotivoConsultaDTO>>> GetAllAsync()
    {
      var list = await _repository.GetListAsync<MotivoConsulta, MotivoConsultaDTO, Guid>(new MotivoConsultaSearchList());
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<MotivoConsultaTableDTO>> GetPaginatedAsync(MotivoConsultaTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0)
      {
        filter.PageNumber = 1;
      }

      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
      var spec = new MotivoConsultaSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<MotivoConsulta, MotivoConsultaTableDTO, Guid>(
        filter.PageNumber,
        filter.PageSize,
        spec
      );
    }

    public async Task<Response<MotivoConsultaDTO>> GetByIdAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<MotivoConsulta, MotivoConsultaDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<MotivoConsultaDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAsync(CreateMotivoConsultaRequest request)
    {
      var matchSpec = new MotivoConsultaMatchDesignacao(request.Designacao ?? string.Empty);
      if (await _repository.ExistsAsync<MotivoConsulta, Guid>(matchSpec))
      {
        return ResponseFactory.Fail<Guid>("Já existe um motivo de consulta com esta designação.");
      }

      var entity = _mapper.Map<MotivoConsulta>(request);
      entity.Id = Guid.NewGuid();

      try
      {
        var created = await _repository.CreateAsync<MotivoConsulta, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateAsync(UpdateMotivoConsultaRequest request, Guid id)
    {
      var existing = await _repository.GetByIdAsync<MotivoConsulta, Guid>(id);
      if (existing == null)
      {
        return ResponseFactory.Fail<Guid>("Motivo de consulta não encontrado.");
      }

      if (!string.Equals(existing.Designacao, request.Designacao, StringComparison.Ordinal))
      {
        var matchSpec = new MotivoConsultaMatchDesignacao(request.Designacao ?? string.Empty);
        if (await _repository.ExistsAsync<MotivoConsulta, Guid>(matchSpec))
        {
          return ResponseFactory.Fail<Guid>("Já existe um motivo de consulta com esta designação.");
        }
      }

      _mapper.Map(request, existing);

      try
      {
        var updated = await _repository.UpdateAsync<MotivoConsulta, Guid>(existing);
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
        var deleted = await _repository.RemoveByIdAsync<MotivoConsulta, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(deleted.Id);
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
          var existing = await _repository.GetByIdAsync<MotivoConsulta, Guid>(id);
          if (existing == null)
          {
            fail.Add($"Motivo de consulta {id} não encontrado.");
            continue;
          }

          var removed = await _repository.RemoveByIdAsync<MotivoConsulta, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Motivo de consulta {id}.");
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
