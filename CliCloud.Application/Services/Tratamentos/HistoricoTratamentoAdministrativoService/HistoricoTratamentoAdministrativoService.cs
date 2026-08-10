using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;
using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.Specifications;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService.Specifications;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;

public class HistoricoTratamentoAdministrativoService(
  IRepositoryAsync repository,
  IUtilizadorDisplayNameResolver utilizadorDisplayNameResolver
) : IHistoricoTratamentoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;
  private readonly IUtilizadorDisplayNameResolver _utilizadorDisplayNameResolver =
    utilizadorDisplayNameResolver;

  public async Task<PaginatedResponse<HistoricoTratamentoTableDTO>> GetPaginatedAsync(
    HistoricoTratamentoTableFilter filter
  )
  {
    filter.Modo = HistoricoTratamentoAdministrativoModos.Normalize(filter.Modo);

    string order =
      filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;

    var spec = new HistoricoTratamentoSearchTable(filter, order);

    return await _repository.GetPaginatedResultsAsync<
      Tratamento,
      HistoricoTratamentoTableDTO,
      Guid
    >(filter.PageNumber, filter.PageSize, spec);
  }

  public async Task<Response<Guid>> PassarParaHistoricoAsync(Guid id)
  {
    Tratamento? tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(id);
    if (tratamento == null || tratamento.DeletedOn != null)
      return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

    DateTime hoje = DateTime.Today;
    tratamento.DataFim = hoje;
    tratamento.ConfDfim = 1;
    _ = await _repository.UpdateAsync<Tratamento, Guid>(tratamento);

    List<SessaoTratamento> sessoes = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesDoTratamentoParaFechoSpec(id)
      )
    ).ToList();

    foreach (SessaoTratamento s in sessoes)
    {
      s.HistSess = 1;
      _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(s);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> ReabrirAsync(Guid id)
  {
    Tratamento? tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(id);
    if (tratamento == null || tratamento.DeletedOn != null)
      return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

    tratamento.DataFim = null;
    tratamento.ConfDfim = 0;
    _ = await _repository.UpdateAsync<Tratamento, Guid>(tratamento);

    List<SessaoTratamento> sessoes = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesDoTratamentoParaFechoSpec(id)
      )
    ).ToList();

    foreach (SessaoTratamento s in sessoes)
    {
      s.HistSess = 0;
      _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(s);
    }

    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<Guid>> DeleteAsync(Guid id)
  {
    string? block = await GetBloqueioEliminacaoPorReciboAsync(id);
    if (block != null)
      return ResponseFactory.Fail<Guid>(block);

    Tratamento? entity = await _repository.GetByIdAsync<Tratamento, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
      return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

    _ = await _repository.RemoveByIdAsync<Tratamento, Guid>(id);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(id);
  }

  public async Task<Response<HistoricoTratamentoObservacoesDTO>> GetObservacoesAsync(Guid id)
  {
    Tratamento? entity = await _repository.GetByIdAsync<Tratamento, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
    {
      return ResponseFactory.Fail<HistoricoTratamentoObservacoesDTO>(
        "Tratamento não encontrado."
      );
    }

    return ResponseFactory.Success(
      new HistoricoTratamentoObservacoesDTO { Observacoes = entity.Obs ?? string.Empty }
    );
  }

  public async Task<Response<Guid>> AppendObservacaoAsync(
    Guid id,
    AppendHistoricoTratamentoObservacaoRequest request
  )
  {
    if (string.IsNullOrWhiteSpace(request.Texto))
      return ResponseFactory.Fail<Guid>("Indique o texto da observação.");

    Tratamento? entity = await _repository.GetByIdAsync<Tratamento, Guid>(id);
    if (entity == null || entity.DeletedOn != null)
      return ResponseFactory.Fail<Guid>("Tratamento não encontrado.");

    string nomeAutor = await _utilizadorDisplayNameResolver.ResolveAsync();
    entity.Obs = AdmissaoObservacoesHelper.FormatarObservacaoAppend(
      request.Texto,
      nomeAutor,
      entity.Obs
    );

    _ = await _repository.UpdateAsync<Tratamento, Guid>(entity);
    _ = await _repository.SaveChangesAsync();
    return ResponseFactory.Success(entity.Id);
  }

  private async Task<string?> GetBloqueioEliminacaoPorReciboAsync(Guid tratamentoId)
  {
    const string msg = "Tratamento já tem recibo associado";

    Tratamento? tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(tratamentoId);
    if (tratamento == null)
      return $"Tratamento {tratamentoId} não encontrado.";

    if (tratamento.ReciboId.HasValue || tratamento.DocumentoId.HasValue)
      return msg;

    List<SessaoTratamento> sessoes = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesTratamentoByTratamentoIdSpec(tratamentoId)
      )
    ).ToList();

    if (sessoes.Any(s => s.ReciboId.HasValue || s.DocumentoId.HasValue))
      return msg;

    return null;
  }
}
