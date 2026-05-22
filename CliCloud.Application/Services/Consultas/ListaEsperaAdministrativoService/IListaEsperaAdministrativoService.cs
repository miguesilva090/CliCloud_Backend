using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;
using CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.Filters;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService;

public interface IListaEsperaAdministrativoService : ITransientService
{
  Task<PaginatedResponse<ListaEsperaTableDTO>> GetPaginatedAsync(ListaEsperaTableFilter filter);
  Task<Response<ListaEsperaDTO>> GetByIdAsync(Guid id);
  Task<Response<Guid>> CreateAsync(CreateListaEsperaRequest request);
  Task<Response<Guid>> UpdateAsync(Guid id, UpdateListaEsperaRequest request);
  Task<Response<Guid>> DeleteAsync(Guid id);
  Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);
  Task<Response<ListaEsperaObservacoesDTO>> GetObservacoesAsync(Guid id);
  Task<Response<Guid>> AppendObservacaoAsync(Guid id, AppendListaEsperaObservacaoRequest request);
  Task<Response<ConverterListaEsperaMarcacaoResultDTO>> ConverterParaMarcacaoAsync(
    Guid id,
    ConverterListaEsperaMarcacaoRequest request
  );
}
