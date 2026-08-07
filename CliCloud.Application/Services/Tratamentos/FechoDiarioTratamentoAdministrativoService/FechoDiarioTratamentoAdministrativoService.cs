using CliCloud.Application.Common;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.DTOs;
using CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService;

public class FechoDiarioTratamentoAdministrativoService(IRepositoryAsync repository)
  : IFechoDiarioTratamentoAdministrativoService
{
  private readonly IRepositoryAsync _repository = repository;

  public async Task<Response<FechoDiarioTratamentoResultDTO>> ExecutarFechoAsync(
    FechoDiarioTratamentoRequest request
  )
  {
    DateTime data = request.Data!.Value.Date;

    List<SessaoTratamento> sessoes = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesParaFechoTratamentoSpec(data)
      )
    ).ToList();

    var result = new FechoDiarioTratamentoResultDTO { TotalElegiveis = sessoes.Count };
    if (sessoes.Count == 0)
    {
      return ResponseFactory.Success(result);
    }

    try
    {
      HashSet<Guid> tratamentosFechados = [];

      foreach (SessaoTratamento sessao in sessoes)
      {
        Tratamento? tratamento = sessao.Tratamento;
        if (tratamento == null || tratamento.DeletedOn != null)
        {
          result.TotalIgnoradas++;
          result.Avisos.Add($"Sessão {sessao.Id}: tratamento em falta.");
          continue;
        }

        int nSessao = sessao.NumSessao ?? 0;
        int totalSess = tratamento.NumSessao ?? 0;
        bool ultima = totalSess > 0 && nSessao >= totalSess;

        if (!ultima)
        {
          sessao.HistSess = 1;
          _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(sessao);
          result.TotalSessoesHistorico++;
          result.TotalProcessadas++;
          continue;
        }

        if (!tratamentosFechados.Contains(tratamento.Id))
        {
          await FecharTratamentoAsync(tratamento, data);
          tratamentosFechados.Add(tratamento.Id);
          result.TotalTratamentosFechados++;
        }

        result.TotalProcessadas++;
      }

      _ = await _repository.SaveChangesAsync();
      return ResponseFactory.Success(result);
    }
    catch (Exception ex)
    {
      _repository.ClearChangeTracker();
      return ResponseFactory.Fail<FechoDiarioTratamentoResultDTO>(
        $"Fecho diário cancelado: {ex.Message}"
      );
    }
  }

  public async Task<Response<int>> ContarElegiveisAsync(DateTime data)
  {
    List<SessaoTratamento> sessoes = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesParaFechoTratamentoSpec(data.Date)
      )
    ).ToList();
    return ResponseFactory.Success(sessoes.Count);
  }

  private async Task FecharTratamentoAsync(Tratamento tratamento, DateTime data)
  {
    tratamento.DataFim = data;
    tratamento.ConfDfim = 1;
    _ = await _repository.UpdateAsync<Tratamento, Guid>(tratamento);

    List<SessaoTratamento> todas = (
      await _repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesDoTratamentoParaFechoSpec(tratamento.Id)
      )
    ).ToList();

    foreach (SessaoTratamento s in todas)
    {
      s.HistSess = 1;
      _ = await _repository.UpdateAsync<SessaoTratamento, Guid>(s);
    }
  }
}