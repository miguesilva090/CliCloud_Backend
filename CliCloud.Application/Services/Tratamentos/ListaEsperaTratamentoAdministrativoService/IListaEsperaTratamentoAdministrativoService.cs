using CliCloud.Application.Common.Marker;

using CliCloud.Application.Common.Wrapper;

using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

using CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Filters;



namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService;



public interface IListaEsperaTratamentoAdministrativoService : ITransientService

{

    Task<PaginatedResponse<ListaEsperaTratamentoTableDTO>> GetPaginatedAsync(ListaEsperaTratamentoTableFilter filter);

    Task<Response<ListaEsperaTratamentoDTO>> GetByIdAsync(Guid id);

    Task<Response<Guid>> CreateAsync(CreateListaEsperaTratamentoRequest request);

    Task<Response<Guid>> UpdateAsync(Guid id, UpdateListaEsperaTratamentoRequest request);

    Task<Response<Guid>> DeleteAsync(Guid id);

    Task<Response<IEnumerable<Guid>>> DeleteMultipleAsync(IEnumerable<Guid> ids);

    Task<Response<ListaEsperaTratamentoObservacoesDTO>> GetObservacoesAsync(Guid id);

    Task<Response<Guid>> AppendObservacaoAsync(Guid id, AppendListaEsperaTratamentoObservacaoRequest request);

    Task<Response<ListaEsperaTratamentoProximoIdentificadorDTO>> GetProximoIdentificadorAsync();

    Task<Response<bool>> VerificarOrdemDisponivelAsync(int ordem, Guid? excludeId = null);

}

