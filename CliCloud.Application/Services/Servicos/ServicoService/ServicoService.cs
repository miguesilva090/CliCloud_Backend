using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Servicos.ServicoService.DTOs;
using CliCloud.Application.Services.Servicos.ServicoService.Filters;
using CliCloud.Application.Services.Servicos.ServicoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.Servicos.ServicoService
{
  public class ServicoService : IServicoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public ServicoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<ServicoDTO>>> GetServicoAsync(string keyword = "")
    {
      var spec = new ServicoSearchList(keyword);
      var list = await _repository.GetListAsync<Servico, ServicoDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<IEnumerable<ServicoLightDTO>>> GetServicoLightAsync(string keyword = "")
    {
      var spec = new ServicoSearchList(keyword);
      var list = await _repository.GetListAsync<Servico, ServicoLightDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<PaginatedResponse<ServicoTableDTO>> GetServicoPaginatedAsync(ServicoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0) filter.PageNumber = 1;
      var order = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var spec = new ServicoSearchTable(filter.Filters ?? [], order);
      return await _repository.GetPaginatedResultsAsync<Servico, ServicoTableDTO, Guid>(filter.PageNumber, filter.PageSize, spec);
    }

    public async Task<Response<IEnumerable<ServicoTableDTO>>> GetAllServicoAsync(ServicoAllFilter? filter)
    {
      try
      {
        filter ??= new ServicoAllFilter();
        var order = filter.GetOrderByString();
        var filters = filter.Filters ?? new List<TableFilter>();
        var spec = new ServicoSearchTable(filters, order);
        var list = await _repository.GetListAsync<Servico, ServicoTableDTO, Guid>(spec);
        return ResponseFactory.Success(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<ServicoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<ServicoDTO>> GetServicoAsync(Guid id)
    {
      try
      {
        var dto = await _repository.GetByIdAsync<Servico, ServicoDTO, Guid>(id);
        return ResponseFactory.Success(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<ServicoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateServicoAsync(CreateServicoRequest request)
    {
      MotivoIsencao? motivoResolvido = null;
      if (request.MotivoIsencaoId is { } mid && mid != Guid.Empty)
      {
        motivoResolvido = await _repository.GetByIdAsync<MotivoIsencao, Guid>(mid);
        if (motivoResolvido == null) return ResponseFactory.Fail<Guid>("Motivo de isenção não encontrado.");
      }

      var entity = _mapper.Map<Servico>(request);
      ApplyMotivoIsencaoFields(entity, motivoResolvido, request.CodigoMotivoIsencao);

      try
      {
        var created = await _repository.CreateAsync<Servico, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> UpdateServicoAsync(UpdateServicoRequest request, Guid id)
    {
      MotivoIsencao? motivoResolvido = null;
      if (request.MotivoIsencaoId is { } mid && mid != Guid.Empty)
      {
        motivoResolvido = await _repository.GetByIdAsync<MotivoIsencao, Guid>(mid);
        if (motivoResolvido == null) return ResponseFactory.Fail<Guid>("Motivo de isenção não encontrado.");
      }

      var existing = await _repository.GetByIdAsync<Servico, Guid>(id);
      if (existing == null) return ResponseFactory.Fail<Guid>("Serviço não encontrado.");

      _ = _mapper.Map(request, existing);
      ApplyMotivoIsencaoFields(existing, motivoResolvido, request.CodigoMotivoIsencao);

      try
      {
        var updated = await _repository.UpdateAsync<Servico, Guid>(existing);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(updated.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteServicoAsync(Guid id)
    {
      try
      {
        var entity = await _repository.RemoveByIdAsync<Servico, Guid>(id);
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success(entity.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<IEnumerable<Guid>>> DeleteMultipleServicoAsync(IEnumerable<Guid> ids)
    {
      var list = ids.ToList();
      var ok = new List<Guid>();
      var fail = new List<string>();

      foreach (var id in list)
      {
        try
        {
          var e = await _repository.GetByIdAsync<Servico, Guid>(id);
          if (e == null) { fail.Add($"Serviço {id} não encontrado."); continue; }
          var removed = await _repository.RemoveByIdAsync<Servico, Guid>(id);
          _ = await _repository.SaveChangesAsync();
          ok.Add(removed.Id);
        }
        catch
        {
          fail.Add($"Serviço {id}.");
          _repository.ClearChangeTracker();
        }
      }

      if (ok.Count == list.Count) return ResponseFactory.Success<IEnumerable<Guid>>(ok);
      if (ok.Count > 0) return ResponseFactory.PartialSuccess<IEnumerable<Guid>>(ok, $"Eliminados {ok.Count} de {list.Count}.");
      return ResponseFactory.Fail<IEnumerable<Guid>>(string.Join("; ", fail));
    }

    private static void ApplyMotivoIsencaoFields(Servico entity, MotivoIsencao? motivoResolvido, int? codigoMotivoIsencaoRequest)
    {
      if (motivoResolvido != null)
      {
        entity.MotivoIsencaoId = motivoResolvido.Id;
        entity.CodigoMotivoIsencao = CodigoMotivoIsencaoLegadoResolver.FromMotivoCodigo(motivoResolvido.Codigo);
      }
      else
      {
        entity.MotivoIsencaoId = null;
        entity.CodigoMotivoIsencao = codigoMotivoIsencaoRequest;
      }
    }
  }
}
