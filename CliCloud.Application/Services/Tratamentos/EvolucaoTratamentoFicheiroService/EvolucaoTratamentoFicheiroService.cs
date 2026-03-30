using AutoMapper;
using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.Specifications;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService
{
  public class EvolucaoTratamentoFicheiroService : IEvolucaoTratamentoFicheiroService
  {
    private readonly IRepositoryAsync _repository;
    private readonly IMapper _mapper;

    public EvolucaoTratamentoFicheiroService(IRepositoryAsync repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Response<IEnumerable<EvolucaoTratamentoFicheiroDTO>>> GetByEvolucaoTratamentoIdAsync(Guid evolucaoTratamentoId)
    {
      var spec = new EvolucaoTratamentoFicheiroByEvolucaoId(evolucaoTratamentoId);
      var list = await _repository.GetListAsync<EvolucaoTratamentoFicheiro, EvolucaoTratamentoFicheiroDTO, Guid>(spec);
      return ResponseFactory.Success(list);
    }

    public async Task<Response<Guid>> CreateAsync(CreateEvolucaoTratamentoFicheiroRequest request)
    {
      var entity = _mapper.Map<EvolucaoTratamentoFicheiro>(request);
      try
      {
        var created = await _repository.CreateAsync<EvolucaoTratamentoFicheiro, Guid>(entity);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(created.Id);
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
        var removed = await _repository.RemoveByIdAsync<EvolucaoTratamentoFicheiro, Guid>(id);
        _ = await _repository.SaveChangesAsync();
        return ResponseFactory.Success(removed.Id);
      }
      catch (Exception ex)
      {
        return ResponseFactory.Fail<Guid>(ex.Message);
      }
    }
  }
}

