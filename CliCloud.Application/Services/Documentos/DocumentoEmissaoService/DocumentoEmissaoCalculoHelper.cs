using CliCloud.Application.Services.Documentos.DocumentoEmissaoService.DTOs;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;


internal static class DocumentoEmissaoCalculoHelper
{
  public const int RegraPrecosSemIvaIncluido = 1;
  public const int RegraPrecosComIvaIncluido = 2;

  public static int ParseRegraFaturacao(string? regrafaturacao) =>
    string.Equals(regrafaturacao?.Trim(), "2", StringComparison.Ordinal)
      ? RegraPrecosComIvaIncluido
      : RegraPrecosSemIvaIncluido;

  public sealed record LinhaCalculoResult(
    decimal TaxaIvaPercentagem,
    decimal TotalLinhaSemIva,
    decimal DescontoValor,
    decimal ValorIncidencia,
    decimal ValorIva,
    decimal SubTotalLinha,
    decimal PercentagemDescontoEfectiva
  );

  public sealed record ResumoIvaLinha(
    decimal TaxaIvaPercentagem,
    decimal ValorIncidencia,
    decimal TotalIva
  );

  public sealed record DocumentoTotaisCalculo(
    decimal Mercadorias,
    decimal Descontos,
    decimal Impostos,
    decimal Total,
    decimal Acerto,
    decimal Retencao,
    decimal APagar,
    IReadOnlyList<ResumoIvaLinha> ResumoIva
  );

  /// <summary>
  /// Percentagem efetiva de desconto (legado <c>calcularTaxaDesconto</c>).
  /// Prioridade: desconto global → composto (cliente × pagamento × tipo1–3).
  /// </summary>
  public static decimal CalcularPercentagemDescontoEfectiva(
    EmitirDocumentoLinhaRequest linha,
    decimal? descontoGlobalPct,
    decimal descontoClientePct,
    decimal descontoPagamentoPct
  )
  {
    if (descontoGlobalPct is > 0)
    {
      return Math.Round(descontoGlobalPct.Value, 2, MidpointRounding.AwayFromZero);
    }

    decimal d1 = linha.DescontoTipo1 ?? 0m;
    decimal d2 = linha.DescontoTipo2 ?? 0m;
    decimal d3 = linha.DescontoTipo3 ?? 0m;

    decimal factor =
      (1m - descontoClientePct / 100m)
      * (1m - descontoPagamentoPct / 100m)
      * (1m - d1 / 100m)
      * (1m - d2 / 100m)
      * (1m - d3 / 100m);

    return Math.Round((1m - factor) * 100m, 2, MidpointRounding.AwayFromZero);
  }

  /// <summary>
  /// <c>TotalLinha</c> persistido (legado grava <c>subTotal</c> da linha).
  /// </summary>
  public static decimal ResolverTotalLinhaPersistencia(
    LinhaCalculoResult calc,
    int regraFaturacao
  ) =>
    regraFaturacao == RegraPrecosComIvaIncluido
      ? calc.SubTotalLinha
      : calc.TotalLinhaSemIva;

  public static LinhaCalculoResult CalcularLinha(
    EmitirDocumentoLinhaRequest linha,
    int regraFaturacao,
    decimal descontoClientePct,
    decimal descontoPagamentoPct,
    decimal? descontoGlobalPct,
    bool isentoIva
  )
  {
    decimal taxa = isentoIva ? 0m : linha.TaxaIvaPercentagem;
    decimal precoUn = linha.PrecoUnitario;

    if (regraFaturacao == RegraPrecosComIvaIncluido && taxa > 0)
    {
      precoUn = precoUn / (1m + taxa / 100m);
    }

    decimal totalLinhaSemIva = precoUn * linha.Quantidade;

    decimal pctEfectiva = CalcularPercentagemDescontoEfectiva(
      linha,
      descontoGlobalPct,
      descontoClientePct,
      descontoPagamentoPct
    );

    // Legado: valorDesconto = totalLinhaSemIva * descontoPerc * 0.01 (arredonda só o desconto)
    decimal descontoValor = Math.Round(
      totalLinhaSemIva * (pctEfectiva / 100m),
      2,
      MidpointRounding.AwayFromZero
    );

    decimal totalSemDesconto = totalLinhaSemIva - descontoValor;

    // Legado: valorIva sem arredondamento intermédio (soma em calculaTotais)
    decimal valorIva = totalSemDesconto * (taxa / 100m);

    decimal subTotalLinha = Math.Round(totalSemDesconto, 2, MidpointRounding.AwayFromZero);
    if (regraFaturacao == RegraPrecosComIvaIncluido)
    {
      subTotalLinha = Math.Round(
        totalSemDesconto + valorIva,
        2,
        MidpointRounding.AwayFromZero
      );
    }

    return new LinhaCalculoResult(
      taxa,
      Math.Round(totalSemDesconto, 2, MidpointRounding.AwayFromZero),
      descontoValor,
      totalSemDesconto,
      valorIva,
      subTotalLinha,
      pctEfectiva
    );
  }

  public static DocumentoTotaisCalculo CalcularTotaisDocumento(
    IReadOnlyList<LinhaCalculoResult> linhas,
    int regraFaturacao,
    decimal acerto,
    decimal retencaoValor
  )
  {
    decimal totalMercadoria = 0m;
    decimal totalDesconto = 0m;
    decimal totalImposto = 0m;

    var porTaxa = linhas.GroupBy(l => l.TaxaIvaPercentagem);
    var resumo = new List<ResumoIvaLinha>();

    foreach (var grupo in porTaxa)
    {
      decimal taxa = grupo.Key;
      decimal valorIncidencia = 0m;
      decimal totalDescGrupo = 0m;
      decimal totalIvaGrupo = 0m;

      foreach (LinhaCalculoResult linha in grupo)
      {
        totalDescGrupo += linha.DescontoValor;

        if (regraFaturacao == RegraPrecosSemIvaIncluido)
        {
          valorIncidencia += linha.ValorIncidencia;
          totalMercadoria += linha.ValorIncidencia + linha.DescontoValor;
        }
        else
        {
          totalIvaGrupo += linha.ValorIva;
          valorIncidencia += linha.ValorIncidencia;
          totalMercadoria += linha.ValorIncidencia + linha.DescontoValor;
        }
      }

      if (regraFaturacao == RegraPrecosSemIvaIncluido && taxa > 0)
      {
        totalIvaGrupo = valorIncidencia * (1m + taxa / 100m) - valorIncidencia;
      }

      totalDesconto += totalDescGrupo;
      totalImposto += totalIvaGrupo;

      resumo.Add(
        new ResumoIvaLinha(
          taxa,
          Math.Round(valorIncidencia, 2, MidpointRounding.AwayFromZero),
          Math.Round(totalIvaGrupo, 2, MidpointRounding.AwayFromZero)
        )
      );
    }

    decimal total = totalMercadoria - totalDesconto + totalImposto;
    decimal aPagar = total + acerto - retencaoValor;

    return new DocumentoTotaisCalculo(
      Math.Round(totalMercadoria, 2, MidpointRounding.AwayFromZero),
      Math.Round(totalDesconto, 2, MidpointRounding.AwayFromZero),
      Math.Round(totalImposto, 2, MidpointRounding.AwayFromZero),
      Math.Round(total, 2, MidpointRounding.AwayFromZero),
      acerto,
      retencaoValor,
      Math.Round(aPagar, 2, MidpointRounding.AwayFromZero),
      resumo
    );
  }

  public static decimal CalcularTotalEstimadoAPagar(
    EmitirDocumentoRequest request,
    int regraFaturacao
  )
  {
    decimal descontoCliente = request.DescontoCliente ?? 0m;
    decimal descontoPagamento = request.DescontoPagamento ?? 0m;
    decimal outros = request.Outros ?? 0m;
    List<LinhaCalculoResult> linhas = request
      .Linhas.Select(l =>
        CalcularLinha(
          l,
          regraFaturacao,
          descontoCliente,
          descontoPagamento,
          request.PercentagemDescontoGlobal,
          request.IsentoIva
        )
      )
      .ToList();

    DocumentoTotaisCalculo semRetencao = CalcularTotaisDocumento(
      linhas,
      regraFaturacao,
      outros,
      0m
    );

    decimal retencao = ResolverRetencaoValor(
      request.RetencaoAtiva,
      request.RetencaoTaxa,
      request.RetencaoValor,
      semRetencao.Total,
      outros
    );

    return CalcularTotaisDocumento(linhas, regraFaturacao, outros, retencao).APagar;
  }

  /// <summary>
  /// Legado: retenção sobre <c>total + acerto</c> (<c>calculaTotais</c>).
  /// </summary>
  public static decimal ResolverRetencaoValor(
    bool retencaoAtiva,
    decimal? retencaoTaxa,
    decimal? retencaoValor,
    decimal totalDocumento,
    decimal acerto
  )
  {
    if (!retencaoAtiva)
    {
      return 0m;
    }

    if (retencaoValor is > 0m)
    {
      return Math.Round(retencaoValor.Value, 2, MidpointRounding.AwayFromZero);
    }

    decimal baseRetencao = totalDocumento + acerto;

    if (retencaoTaxa is > 0m)
    {
      return Math.Round(
        baseRetencao * (retencaoTaxa.Value / 100m),
        2,
        MidpointRounding.AwayFromZero
      );
    }

    return 0m;
  }

  /// <summary>Compatibilidade com chamadas antigas (regra 1, sem descontos de cabeçalho).</summary>
  public static decimal CalcularTotalLinhaComIva(
    EmitirDocumentoLinhaRequest linha,
    decimal? descontoGlobalPct,
    int regraFaturacao = RegraPrecosSemIvaIncluido,
    decimal descontoClientePct = 0m,
    decimal descontoPagamentoPct = 0m,
    bool isentoIva = false
  )
  {
    LinhaCalculoResult r = CalcularLinha(
      linha,
      regraFaturacao,
      descontoClientePct,
      descontoPagamentoPct,
      descontoGlobalPct,
      isentoIva
    );
    return r.SubTotalLinha;
  }
}
