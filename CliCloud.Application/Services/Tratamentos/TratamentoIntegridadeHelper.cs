using CliCloud.Application.Common;
using CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.Specifications;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos;

internal static class TratamentoIntegridadeHelper
{
  public static void NormalizarTratamento(Tratamento tratamento)
  {
    tratamento.NumSessao = tratamento.NumSessao.GetValueOrDefault() < 0 ? 0 : tratamento.NumSessao;
    tratamento.NFalta ??= 0;
    tratamento.NFaltaCons ??= 0;
    tratamento.NAltSess ??= 0;

    if (tratamento.DataFim.HasValue && tratamento.DataInic.HasValue &&
        tratamento.DataFim.Value.Date < tratamento.DataInic.Value.Date)
    {
      tratamento.DataFim = tratamento.DataInic;
    }
  }

  public static async Task GarantirSessoesPlaneadasAsync(
    Tratamento tratamento,
    IRepositoryAsync repository,
    CancellationToken cancellationToken = default
  )
  {
    int total = tratamento.NumSessao.GetValueOrDefault();
    if (total <= 0)
    {
      return;
    }

    List<SessaoTratamento> existentes = (
      await repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesTratamentoByTratamentoIdSpec(tratamento.Id),
        cancellationToken
      )
    ).ToList();

    HashSet<int> numeros = existentes
      .Where(x => x.NumSessao.HasValue)
      .Select(x => x.NumSessao!.Value)
      .ToHashSet();

    for (int i = 1; i <= total; i++)
    {
      if (numeros.Contains(i))
      {
        continue;
      }

      _ = await repository.CreateAsync<SessaoTratamento, Guid>(
        new SessaoTratamento
        {
          TratamentoId = tratamento.Id,
          NumSessao = i,
          HoraFisio = tratamento.HoraFisio,
          HoraAux = tratamento.HoraAux,
          HoraOutro = tratamento.HoraOutro,
          Duracao = tratamento.DuracaoTotal,
          FisioterapeutaId = tratamento.FisioterapeutaId,
          AuxiliarId = tratamento.AuxiliarId,
          OutroTecnicoId = tratamento.OutroTecnicoId,
          Faltou = 0,
          CompensaFalta = 0,
          Desmarcado = 0,
        }
      );
    }
  }

  public static async Task RecalcularFaltasAsync(
    Guid tratamentoId,
    IRepositoryAsync repository,
    CancellationToken cancellationToken = default
  )
  {
    Tratamento tratamento = await repository.GetByIdAsync<Tratamento, Guid>(
      tratamentoId,
      cancellationToken: cancellationToken
    );
    if (tratamento == null || tratamento.DeletedOn != null)
    {
      return;
    }

    List<SessaoTratamento> sessoes = (
      await repository.GetListAsync<SessaoTratamento, Guid>(
        new SessoesTratamentoByTratamentoIdSpec(tratamentoId),
        cancellationToken
      )
    ).ToList();

    int totalFaltas = 0;
    int consecutivas = 0;
    foreach (SessaoTratamento sessao in sessoes)
    {
      bool faltaContabilizavel =
        sessao.Faltou == 1 &&
        sessao.CompensaFalta != 1 &&
        sessao.Desmarcado != 1;

      if (faltaContabilizavel)
      {
        totalFaltas++;
        consecutivas++;
      }
      else
      {
        consecutivas = 0;
      }
    }

    tratamento.NFalta = totalFaltas;
    tratamento.NFaltaCons = consecutivas;
    tratamento.NumSessao ??= sessoes
      .Where(x => x.NumSessao.HasValue)
      .Select(x => x.NumSessao!.Value)
      .DefaultIfEmpty(0)
      .Max();

    _ = await repository.UpdateAsync<Tratamento, Guid>(tratamento);
  }
}
