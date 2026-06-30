using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectCorrecaoLotesExecutor(ApplicationDbContext dbContext) : ILoteDirectCorrecaoLotesExecutor
{
  private const int MaxCredenciaisPorLote = 30;
  private const int TipoLoteValorExamesSemPapel = 97;

  private sealed record PrecoServico(
    decimal ValorServico,
    decimal ValorUtente,
    decimal ValorOrganismo
  );

  public async Task<CorrigirLotesResultDTO> ExecutarAsync(int ano, int mes, CancellationToken cancellationToken = default)
  {
    List<string> avisos = [];

    await using IDbContextTransaction tx =
      await dbContext.Database.BeginTransactionAsync(cancellationToken);

    int? tipoExamesSemPapelId = await EnsureTipoLoteExamesSemPapelAsync(cancellationToken);

    List<LoteDirect> cabecalhos = await dbContext
      .Set<LoteDirect>()
      .Where(x => !x.Historico && x.Ano == ano && x.Mes == mes)
      .OrderBy(x => x.CodigoOrganismo)
      .ThenBy(x => x.TipoServico)
      .ThenBy(x => x.TipoLote)
      .ThenBy(x => x.Id)
      .ToListAsync(cancellationToken);

    if (cabecalhos.Count == 0)
    {
      await dbContext
        .Set<LoteDirectDetalhe>()
        .Where(x => x.Ano == ano && x.Mes == mes)
        .ExecuteDeleteAsync(cancellationToken);

      await dbContext
        .Set<LoteDirectAgregado>()
        .Where(x => x.Ano == ano && x.Mes == mes)
        .ExecuteDeleteAsync(cancellationToken);

      await tx.CommitAsync(cancellationToken);
      return new CorrigirLotesResultDTO
      {
        Ano = ano,
        Mes = mes,
        CabecalhosProcessados = 0,
        AgregadosCriados = 0,
        DetalhesCriados = 0,
        Avisos = ["Nenhum lançamento encontrado para o período."],
      };
    }

    int semOrganismo = cabecalhos.Count(x => x.CodigoOrganismo is null);
    if (semOrganismo > 0)
      avisos.Add($"{semOrganismo} lançamento(s) sem organismo.");

    await dbContext
      .Set<LoteDirectDetalhe>()
      .Where(x => x.Ano == ano && x.Mes == mes)
      .ExecuteDeleteAsync(cancellationToken);

    await dbContext
      .Set<LoteDirectAgregado>()
      .Where(x => x.Ano == ano && x.Mes == mes)
      .ExecuteDeleteAsync(cancellationToken);

    RenumerarLotes(cabecalhos, tipoExamesSemPapelId);
    AplicarRegraExamesSemPapel(cabecalhos, tipoExamesSemPapelId);

    foreach (LoteDirect cab in cabecalhos)
      cab.IndiceLote = null;

    List<LoteDirectLinha> linhas = await dbContext
      .Set<LoteDirectLinha>()
      .Join(
        dbContext.Set<LoteDirect>().Where(x => !x.Historico && x.Ano == ano && x.Mes == mes),
        linha => linha.LoteDirectId,
        cabecalho => cabecalho.Id,
        (linha, _) => linha
      )
      .ToListAsync(cancellationToken);

    List<LoteDirectLinha789> linhas789 = await dbContext
      .Set<LoteDirectLinha789>()
      .Join(
        dbContext.Set<LoteDirect>().Where(x => !x.Historico && x.Ano == ano && x.Mes == mes),
        linha => linha.LoteDirectId,
        cabecalho => cabecalho.Id,
        (linha, _) => linha
      )
      .ToListAsync(cancellationToken);

    Dictionary<(int CodigoOrganismo, Guid ServicoId), PrecoServico> precos =
      await CarregarPrecosAsync(cabecalhos, linhas, linhas789, cancellationToken);

    foreach (LoteDirect cab in cabecalhos)
    {
      if (cab.CodigoOrganismo is null)
        continue;

      foreach (LoteDirectLinha linha in linhas.Where(x => x.LoteDirectId == cab.Id))
      {
        if (!precos.TryGetValue((cab.CodigoOrganismo.Value, linha.ServicoId), out PrecoServico? preco))
          continue;

        AplicarPrecoLinha(cab, linha, preco);
      }

      foreach (LoteDirectLinha789 linha in linhas789.Where(x => x.LoteDirectId == cab.Id))
      {
        if (!precos.TryGetValue((cab.CodigoOrganismo.Value, linha.ServicoId), out PrecoServico? preco))
          continue;

        AplicarPrecoLinha789(cab, linha, preco);
      }
    }

    RecalcularTotaisCabecalho(cabecalhos, linhas, linhas789);

    List<LoteDirectAgregado> agregados = CriarAgregados(cabecalhos, linhas, linhas789, ano, mes);
    dbContext.Set<LoteDirectAgregado>().AddRange(agregados);
    await dbContext.SaveChangesAsync(cancellationToken);

    AtualizarIndices(cabecalhos, agregados);

    List<LoteDirectDetalhe> detalhes = CriarDetalhes(cabecalhos, linhas, linhas789, agregados);
    dbContext.Set<LoteDirectDetalhe>().AddRange(detalhes);

    await dbContext.SaveChangesAsync(cancellationToken);
    await tx.CommitAsync(cancellationToken);

    return new CorrigirLotesResultDTO
    {
      Ano = ano,
      Mes = mes,
      CabecalhosProcessados = cabecalhos.Count,
      AgregadosCriados = agregados.Count,
      DetalhesCriados = detalhes.Count,
      Avisos = avisos,
    };
  }

  private async Task<int?> EnsureTipoLoteExamesSemPapelAsync(CancellationToken cancellationToken)
  {
    int? tipoExamesSemPapelId = await dbContext
      .Set<TipoLote>()
      .AsNoTracking()
      .Where(x => x.Valor == TipoLoteValorExamesSemPapel)
      .Select(x => (int?)x.Id)
      .FirstOrDefaultAsync(cancellationToken);

    if (tipoExamesSemPapelId is not null)
      return tipoExamesSemPapelId;

    int nextId = (await dbContext.Set<TipoLote>().MaxAsync(x => (int?)x.Id, cancellationToken) ?? 0) + 1;
    TipoLote tipoLote = new()
    {
      Id = nextId,
      Valor = TipoLoteValorExamesSemPapel,
      Designa = "EXAMES SEM PAPEL",
    };

    await dbContext.Set<TipoLote>().AddAsync(tipoLote, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return tipoLote.Id;
  }

  private static void RenumerarLotes(List<LoteDirect> cabecalhos, int? tipoExamesSemPapelId)
  {
    List<LoteDirect> elegiveis = cabecalhos
      .Where(x => tipoExamesSemPapelId is null || x.TipoLote != tipoExamesSemPapelId)
      .ToList();

    int? organismo = null;
    int? tipoServico = null;
    int? tipoLote = null;
    int numeroLote = 1;
    int contador = 1;

    foreach (LoteDirect cab in elegiveis)
    {
      if (organismo != cab.CodigoOrganismo || tipoServico != cab.TipoServico || tipoLote != cab.TipoLote)
      {
        numeroLote = 1;
        contador = 1;
      }
      else if (contador > MaxCredenciaisPorLote)
      {
        numeroLote++;
        contador = 1;
      }

      cab.NumeroLote = numeroLote;
      organismo = cab.CodigoOrganismo;
      tipoServico = cab.TipoServico;
      tipoLote = cab.TipoLote;
      contador++;
    }
  }

  private static void AplicarRegraExamesSemPapel(List<LoteDirect> cabecalhos, int? tipoExamesSemPapelId)
  {
    if (tipoExamesSemPapelId is null)
      return;

    foreach (LoteDirect cab in cabecalhos.Where(x => x.TipoLote == tipoExamesSemPapelId && x.NumeroLote > 1))
      cab.NumeroLote = 1;
  }

  private async Task<Dictionary<(int CodigoOrganismo, Guid ServicoId), PrecoServico>> CarregarPrecosAsync(
    List<LoteDirect> cabecalhos,
    List<LoteDirectLinha> linhas,
    List<LoteDirectLinha789> linhas789,
    CancellationToken cancellationToken
  )
  {
    HashSet<Guid> servicoIds = linhas
      .Select(x => x.ServicoId)
      .Concat(linhas789.Select(x => x.ServicoId))
      .ToHashSet();

    HashSet<int> codigosOrganismo = cabecalhos
      .Where(x => x.CodigoOrganismo.HasValue)
      .Select(x => x.CodigoOrganismo!.Value)
      .ToHashSet();

    if (servicoIds.Count == 0 || codigosOrganismo.Count == 0)
      return [];

    var organismosRaw = await dbContext
      .Set<Organismo>()
      .AsNoTracking()
      .Where(x => x.CodigoULSNova != null)
      .Select(x => new { x.Id, Codigo = x.CodigoULSNova!.Value })
      .ToListAsync(cancellationToken);

    var organismos = organismosRaw
      .Where(x => codigosOrganismo.Contains(x.Codigo))
      .ToList();

    HashSet<Guid> organismoIds = organismos.Select(x => x.Id).ToHashSet();
    if (organismoIds.Count == 0)
      return [];

    var precosRaw = await dbContext
      .Set<SubsistemaServico>()
      .AsNoTracking()
      .Where(x => !x.Inativo)
      .Select(x => new { x.OrganismoId, x.ServicoId, x.ValorServico, x.ValorUtente, x.ValorOrganismo })
      .ToListAsync(cancellationToken);

    var precos = precosRaw
      .Where(x => organismoIds.Contains(x.OrganismoId) && servicoIds.Contains(x.ServicoId))
      .ToList();

    Dictionary<Guid, int> codigoPorOrganismoId = organismos.ToDictionary(x => x.Id, x => x.Codigo);

    return precos
      .Where(x => codigoPorOrganismoId.ContainsKey(x.OrganismoId))
      .ToDictionary(
        x => (codigoPorOrganismoId[x.OrganismoId], x.ServicoId),
        x => new PrecoServico(x.ValorServico, x.ValorUtente, x.ValorOrganismo)
      );
  }

  private static void AplicarPrecoLinha(LoteDirect cab, LoteDirectLinha linha, PrecoServico preco)
  {
    linha.ValorUnitario = preco.ValorServico;
    bool isIsento = cab.Isencao == 1;

    linha.ValorUtenteOriginal = isIsento ? 0 : preco.ValorUtente;
    linha.ValorInstituicaoOriginal = isIsento ? preco.ValorServico : preco.ValorOrganismo;
    linha.ValorUtente = linha.ValorUtenteOriginal * linha.Quantidade;
    linha.ValorInstituicao = linha.ValorInstituicaoOriginal * linha.Quantidade;
  }

  private static void AplicarPrecoLinha789(LoteDirect cab, LoteDirectLinha789 linha, PrecoServico preco)
  {
    linha.ValorUnitario = preco.ValorServico;
    bool isIsento = cab.Isencao == 1;

    linha.ValorUtenteOriginal = isIsento ? 0 : preco.ValorUtente;
    linha.ValorInstituicaoOriginal = isIsento ? preco.ValorServico : preco.ValorOrganismo;
    linha.ValorUtente = linha.ValorUtenteOriginal * linha.Quantidade;
    linha.ValorInstituicao = linha.ValorInstituicaoOriginal * linha.Quantidade;
  }

  private static void RecalcularTotaisCabecalho(
    List<LoteDirect> cabecalhos,
    List<LoteDirectLinha> linhas,
    List<LoteDirectLinha789> linhas789
  )
  {
    Dictionary<Guid, List<LoteDirectLinha>> porCab = linhas
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());
    Dictionary<Guid, List<LoteDirectLinha789>> porCab789 = linhas789
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());

    foreach (LoteDirect cab in cabecalhos)
    {
      porCab.TryGetValue(cab.Id, out List<LoteDirectLinha>? l1);
      porCab789.TryGetValue(cab.Id, out List<LoteDirectLinha789>? l2);
      l1 ??= [];
      l2 ??= [];

      decimal taxaLinhas = l1.Sum(x => x.ValorUtente) + l2.Sum(x => x.ValorUtente);
      decimal subtotal = l1.Sum(x => x.ValorUnitario * x.Quantidade) + l2.Sum(x => x.ValorUnitario * x.Quantidade);

      cab.ValorTaxasLinhas = taxaLinhas;
      cab.Subtotal = subtotal;
      cab.ValorTaxas = (cab.TaxaConsulta ?? 0) + taxaLinhas;
      cab.ValorTotal = (cab.ValorConsulta ?? 0) + subtotal;
    }
  }

  private static List<LoteDirectAgregado> CriarAgregados(
    List<LoteDirect> cabecalhos,
    List<LoteDirectLinha> linhas,
    List<LoteDirectLinha789> linhas789,
    int ano,
    int mes
  )
  {
    Dictionary<Guid, List<LoteDirectLinha>> porCab = linhas
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());
    Dictionary<Guid, List<LoteDirectLinha789>> porCab789 = linhas789
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());

    return cabecalhos
      .GroupBy(x => new
      {
        NumeroLote = x.NumeroLote ?? 0,
        CodigoOrganismo = x.CodigoOrganismo ?? 0,
        TipoLote = x.TipoLote ?? 0,
        TipoServico = x.TipoServico ?? 0,
      })
      .Select(grupo =>
      {
        List<LoteDirect> membros = grupo.ToList();
        int quantidade = 0;
        decimal valor = 0;
        decimal valorTaxa = 0;
        int? isencao = null;

        foreach (LoteDirect cab in membros)
        {
          porCab.TryGetValue(cab.Id, out List<LoteDirectLinha>? l1);
          porCab789.TryGetValue(cab.Id, out List<LoteDirectLinha789>? l2);
          l1 ??= [];
          l2 ??= [];

          quantidade += l1.Sum(x => x.Quantidade) + l2.Sum(x => x.Quantidade) + (cab.QuantidadeConsulta ?? 0);

          valor += l1.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + l2.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + (cab.ValorConsulta ?? 0);

          valorTaxa += l1.Sum(x => x.ValorUtente) + l2.Sum(x => x.ValorUtente) + (cab.TaxaConsulta ?? 0);

          isencao = Math.Max(isencao ?? cab.Isencao ?? 0, cab.Isencao ?? 0);
        }

        return new LoteDirectAgregado
        {
          Id = Guid.NewGuid(),
          NumeroLote = grupo.Key.NumeroLote,
          Ano = ano,
          Mes = mes,
          CodigoOrganismo = grupo.Key.CodigoOrganismo,
          TipoLote = grupo.Key.TipoLote,
          TipoServico = grupo.Key.TipoServico,
          DataLote = DateTime.UtcNow,
          Quantidade = quantidade,
          Valor = valor,
          ValorTaxa = valorTaxa,
          Isencao = isencao,
          NumeroRequisicoes = membros.Count,
        };
      })
      .ToList();
  }

  private static void AtualizarIndices(List<LoteDirect> cabecalhos, List<LoteDirectAgregado> agregados)
  {
    foreach (LoteDirect cab in cabecalhos)
    {
      LoteDirectAgregado? agregado = agregados.FirstOrDefault(x =>
        x.NumeroLote == (cab.NumeroLote ?? 0)
        && x.CodigoOrganismo == (cab.CodigoOrganismo ?? 0)
        && x.TipoLote == (cab.TipoLote ?? 0)
        && x.TipoServico == (cab.TipoServico ?? 0)
      );

      if (agregado is null)
        continue;

      cab.IndiceLote = agregado.Indice;
    }
  }

  private static List<LoteDirectDetalhe> CriarDetalhes(
    List<LoteDirect> cabecalhos,
    List<LoteDirectLinha> linhas,
    List<LoteDirectLinha789> linhas789,
    List<LoteDirectAgregado> agregados
  )
  {
    Dictionary<Guid, List<LoteDirectLinha>> porCab = linhas
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());
    Dictionary<Guid, List<LoteDirectLinha789>> porCab789 = linhas789
      .GroupBy(x => x.LoteDirectId)
      .ToDictionary(g => g.Key, g => g.ToList());

    List<LoteDirectDetalhe> detalhes = [];

    foreach (LoteDirect cab in cabecalhos)
    {
      LoteDirectAgregado? agregado = agregados.FirstOrDefault(x =>
        x.NumeroLote == (cab.NumeroLote ?? 0)
        && x.CodigoOrganismo == (cab.CodigoOrganismo ?? 0)
        && x.TipoLote == (cab.TipoLote ?? 0)
        && x.TipoServico == (cab.TipoServico ?? 0)
      );

      if (agregado is null)
        continue;

      porCab.TryGetValue(cab.Id, out List<LoteDirectLinha>? l1);
      porCab789.TryGetValue(cab.Id, out List<LoteDirectLinha789>? l2);
      l1 ??= [];
      l2 ??= [];

      detalhes.Add(
        new LoteDirectDetalhe
        {
          Id = Guid.NewGuid(),
          LoteDirectAgregadoId = agregado.Id,
          LoteDirectId = cab.Id,
          Indice = agregado.Indice,
          NumeroLote = cab.NumeroLote ?? 0,
          Ano = cab.Ano ?? 0,
          Mes = cab.Mes ?? 0,
          CodigoOrganismo = cab.CodigoOrganismo ?? 0,
          TipoServico = cab.TipoServico ?? 0,
          TipoLote = cab.TipoLote ?? 0,
          Credencial = cab.Credencial,
          Quantidade = l1.Sum(x => x.Quantidade) + l2.Sum(x => x.Quantidade) + (cab.QuantidadeConsulta ?? 0),
          Valor = l1.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + l2.Sum(x => x.ValorUtente + x.ValorInstituicao)
            + (cab.ValorConsulta ?? 0),
          ValorTaxa = l1.Sum(x => x.ValorUtente) + l2.Sum(x => x.ValorUtente) + (cab.TaxaConsulta ?? 0),
          Isencao = cab.Isencao,
          Data = cab.DataFim,
        }
      );
    }

    return detalhes;
  }
}
