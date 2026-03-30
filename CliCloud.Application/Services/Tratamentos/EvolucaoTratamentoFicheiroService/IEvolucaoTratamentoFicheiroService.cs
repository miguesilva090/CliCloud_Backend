using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService
{
  public interface IEvolucaoTratamentoFicheiroService : ITransientService
  {
    Task<Response<IEnumerable<EvolucaoTratamentoFicheiroDTO>>> GetByEvolucaoTratamentoIdAsync(Guid evolucaoTratamentoId);
    Task<Response<Guid>> CreateAsync(CreateEvolucaoTratamentoFicheiroRequest request);
    Task<Response<Guid>> DeleteAsync(Guid id);
  }
}

