using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Filters;
using CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService.Specifications;
using CliCloud.Application.Utility;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.AdmissaoTratamentoAdministrativoService;

public class AdmissaoTratamentoAdministrativoService(IRepositoryAsync repository)
  : IAdmissaoTratamentoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;

  public async Task<PaginatedResponse<AdmissaoTratamentoTableDTO>> GetPaginatedAsync(
    AdmissaoTratamentoTableFilter filter
  )
  {
    if (filter.Filters?.Count > 0)
    {
      filter.PageNumber = 1;
    }

    string order =
      filter.Sorting != null ? GSHelpers.GenerateOrderByString(filter) : string.Empty;
    var spec = new AdmissaoTratamentoSearchTable(filter, order);

    return await _repository.GetPaginatedResultsAsync<
      SessaoTratamento,
      AdmissaoTratamentoTableDTO,
      Guid
    >(filter.PageNumber, filter.PageSize, spec);
  }

  public async Task<Response<Guid>> UpdateSituacaoAsync(
    Guid id,
    UpdateAdmissaoTratamentoSituacaoRequest request
  )
  {
    SessaoTratamento? entity = await _repository.GetByIdAsync<SessaoTratamento, Guid>(id);
    if (entity == null)
    {
      return ResponseFactory.Fail<Guid>("Sessão de tratamento não encontrada.");
    }

    string campo = request.Campo.Trim().ToLowerInvariant();
    int valor = request.Valor;
    int faltouAntes = entity.Faltou ?? 0;

    switch (campo)
    {
      case "confirmado":
        if (valor == 1 && (entity.Faltou ?? 0) == 1)
        {
          return ResponseFactory.Fail<Guid>(
            "Não é possível marcar confirmado: a sessão já está marcada como faltou."
          );
        }
        entity.Confirmado = valor;
        break;

      case "efetuado":
        if (valor == 1 && (entity.Faltou ?? 0) == 1)
        {
          return ResponseFactory.Fail<Guid>(
            "Não é possível marcar efectuado: a sessão já está marcada como faltou."
          );
        }
        entity.Efetuado = valor;
        break;

      case "faltou":
        if (valor == 1 && (entity.Efetuado ?? 0) == 1)
        {
          return ResponseFactory.Fail<Guid>(
            "Não é possível marcar faltou: a sessão já está marcada como efectuada."
          );
        }
        if (valor == 1 && (entity.Confirmado ?? 0) == 1)
        {
          return ResponseFactory.Fail<Guid>(
            "Não é possível marcar faltou: a sessão já está marcada como confirmada. Desmarque confirmado primeiro."
          );
        }
        entity.Faltou = valor;
        if (valor == 1)
        {
          entity.Confirmado = 0;
        }
        break;

      default:
        return ResponseFactory.Fail<Guid>("Campo de situação inválido.");
    }

    int faltouDepois = entity.Faltou ?? 0;
    if (faltouDepois != faltouAntes)
    {
      Tratamento? tratamento = await _repository.GetByIdAsync<Tratamento, Guid>(
        entity.TratamentoId
      );
      if (tratamento != null)
      {
        int n = tratamento.NFalta ?? 0;
        tratamento.NFalta =
          faltouDepois == 1 ? n + 1 : Math.Max(0, n - 1);
        _ = await _repository.UpdateAsync<Tratamento, Guid>(tratamento);
      }
    }

    try
    {
      _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(entity);
      _ = await _repository.SaveChangesAsync();
    }
    catch (Exception ex) when (ex.Message.Contains("Nada a ser atualizado"))
    {
      return ResponseFactory.Success(entity.Id);
    }

    return ResponseFactory.Success(entity.Id);
  }
}
