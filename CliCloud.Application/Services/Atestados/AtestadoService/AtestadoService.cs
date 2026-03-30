using System.Linq;
using AutoMapper;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Atestados;
using CliCloud.Application.Services.Atestados.AtestadoService.DTOs;
using CliCloud.Application.Services.Atestados.AtestadoService.Filters;
using CliCloud.Application.Services.Atestados.AtestadoService.Specifications;

namespace CliCloud.Application.Services.Atestados.AtestadoService
{
  public class AtestadoService : IAtestadoService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public AtestadoService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<PaginatedResponse<AtestadoTableDTO>> GetAtestadoPaginatedAsync(AtestadoTableFilter filter)
    {
      if (filter.Filters != null && filter.Filters.Count > 0)
        filter.PageNumber = 1;

      string dynamicOrder = filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : "";
      var specification = new AtestadoSearchTable(filter.Filters ?? [], dynamicOrder);
      PaginatedResponse<AtestadoTableDTO> pagedResponse = await _repository.GetPaginatedResultsAsync<Atestado, AtestadoTableDTO, Guid>(filter.PageNumber, filter.PageSize, specification);
      return pagedResponse;
    }

    public async Task<Response<IEnumerable<AtestadoTableDTO>>> GetAllAtestadoAsync(AtestadoAllFilter filter)
    {
      try
      {
        filter ??= new AtestadoAllFilter();
        string dynamicOrder = filter.GetOrderByString();
        List<TableFilter> tableFilters = filter.Filters ?? [];
        var specification = new AtestadoSearchTable(tableFilters, dynamicOrder);
        IEnumerable<AtestadoTableDTO> list = await _repository.GetListAsync<Atestado, AtestadoTableDTO, Guid>(specification);
        return ResponseFactory.Success<IEnumerable<AtestadoTableDTO>>(list);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<IEnumerable<AtestadoTableDTO>>(ex.Message);
      }
    }

    public async Task<Response<AtestadoDTO>> GetAtestadoAsync(Guid id)
    {
      try
      {
        AtestadoDTO dto = await _repository.GetByIdAsync<Atestado, AtestadoDTO, Guid>(id);
        return ResponseFactory.Success<AtestadoDTO>(dto);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<AtestadoDTO>(ex.Message);
      }
    }

    public async Task<Response<Guid>> CreateAtestadoAsync(CreateAtestadoRequest request)
    {
      Atestado atestado = _mapper.Map<CreateAtestadoRequest, Atestado>(request);
      atestado.Id = Guid.NewGuid();
      atestado.EstadoEnvio = 0;

      try
      {
        _ = await _repository.CreateAsync<Atestado, Guid>(atestado);

        foreach (var item in request.Categorias)
        {
          var cat = new AtestadoCategoria
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoId = item.CartaConducaoId,
            Apto = item.Apto,
            AptoGrupo2 = item.AptoGrupo2
          };
          _ = await _repository.CreateAsync<AtestadoCategoria, Guid>(cat);
        }

        foreach (var item in request.Restricoes)
        {
          var rest = new AtestadoRestricao
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoRestricaoId = item.CartaConducaoRestricaoId,
            CartaConducaoId = item.CartaConducaoId,
            Anotacoes = item.Anotacoes
          };
          _ = await _repository.CreateAsync<AtestadoRestricao, Guid>(rest);
        }

        foreach (var item in request.RestricoesAnteriores)
        {
          var restAnt = new AtestadoRestricaoAnterior
          {
            Id = Guid.NewGuid(),
            AtestadoId = atestado.Id,
            CartaConducaoRestricaoId = item.CartaConducaoRestricaoId,
            Anotacoes = item.Anotacoes
          };
          _ = await _repository.CreateAsync<AtestadoRestricaoAnterior, Guid>(restAnt);
        }

        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(atestado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }

    public async Task<Response<Guid>> DeleteAtestadoAsync(Guid id)
    {
      try
      {
        var categorias = await _repository.GetListAsync<AtestadoCategoria, Guid>(new AtestadoCategoriaByAtestadoId(id));
        foreach (var c in categorias)
          await _repository.RemoveAsync<AtestadoCategoria, Guid>(c);

        var restricoes = await _repository.GetListAsync<AtestadoRestricao, Guid>(new AtestadoRestricaoByAtestadoId(id));
        foreach (var r in restricoes)
          await _repository.RemoveAsync<AtestadoRestricao, Guid>(r);

        var restricoesAnt = await _repository.GetListAsync<AtestadoRestricaoAnterior, Guid>(new AtestadoRestricaoAnteriorByAtestadoId(id));
        foreach (var ra in restricoesAnt)
          await _repository.RemoveAsync<AtestadoRestricaoAnterior, Guid>(ra);

        Atestado? atestado = await _repository.RemoveByIdAsync<Atestado, Guid>(id);
        if (atestado == null)
          return ResponseFactory.Fail<Guid>("Atestado não encontrado");
        await _repository.SaveChangesAsync();
        return ResponseFactory.Success<Guid>(atestado.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}
