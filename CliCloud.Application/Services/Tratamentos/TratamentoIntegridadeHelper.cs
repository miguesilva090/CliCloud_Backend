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
    )
    .OrderBy(s => s.Data ?? DateTime.MaxValue)
    .ThenBy(s => s.NumSessao ?? int.MaxValue)
    .ToList();

    int faltas = sessoes.Count(s => s.Faltou == 1 && s.Desmarcado != 1);
    int compensacoes = sessoes.Count(s => s.CompensaFalta == 1 && s.Desmarcado != 1);

    tratamento.NFalta = Math.Max(0, faltas - compensacoes);

    int pool = compensacoes;
    int consecutivas = 0;
    int maxCons = 0;

    foreach (SessaoTratamento sessao in sessoes)
    {
      if (sessao.Desmarcado == 1) continue;
      if (sessao.CompensaFalta == 1) continue;
      if (sessao.Faltou == 1)
      {
        if (pool > 0)
        {
          pool--;
          consecutivas = 0;
        }
        else
        {
          consecutivas++;
          if (consecutivas > maxCons) maxCons = consecutivas;
        }
      }
      else
      {
        consecutivas = 0;
      }
    }


    tratamento.NFaltaCons = maxCons;
    tratamento.NumSessao = Math.Max(
      sessoes.Count,
      sessoes
        .Where(x => x.NumSessao.HasValue)
        .Select(x => x.NumSessao!.Value)
        .DefaultIfEmpty(0)
        .Max()
    );

    _ = await repository.UpdateAsync<Tratamento, Guid>(tratamento);
  }
}
