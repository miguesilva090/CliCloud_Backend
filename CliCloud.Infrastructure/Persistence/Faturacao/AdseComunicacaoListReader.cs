using CliCloud.Application.Services.Faturacao.AdseComunicacaoService;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class AdseComunicacaoListReader(ApplicationDbContext db) : IAdseComunicacaoListReader
{
    public async Task<AdseComunicacaoPaginatedDTO> ObterPaginadoAsync(
        Guid clinicaId, Guid organismoAdseId, string tipo, AdseComunicacaoTableFilter filter, CancellationToken ct)
    {
        List<AdseComunicacaoLinhaDTO> linhas = tipo switch
        {
            AdseEstados.TipoTratamentos => await TratamentosAsync(clinicaId, organismoAdseId, filter, ct),
            AdseEstados.TipoConsultas => await ConsultasAsync(clinicaId, organismoAdseId, filter, ct),
            AdseEstados.TipoExames => [],
            _ => [],
        };

        AplicarFiltrosPosQuery(linhas, filter);

        int total = linhas.Count;
        int skip = (filter.PageNumber - 1) * filter.PageSize;
        List<AdseComunicacaoLinhaDTO> page = linhas.Skip(skip).Take(filter.PageSize).ToList();

        return new AdseComunicacaoPaginatedDTO
        {
            Linhas = page,
            TotalCount = total,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalFaturaPagina = page.Sum(x => x.ValorFatura),
            TotalFatura = linhas.Sum(x => x.ValorFatura),
            TotalAdsePagina = page.Sum(x => x.ValorAdse),
            TotalAdse = linhas.Sum(x => x.ValorAdse),
        };
    }

    private async Task<List<AdseComunicacaoLinhaDTO>> TratamentosAsync(
        Guid clinicaId, Guid organismoId, AdseComunicacaoTableFilter filter, CancellationToken ct)
    {
        (DateTime ini, DateTime fim) = IntervaloFatura(filter);

        var rows = await (
            from sess in db.SessoesTratamento.AsNoTracking()
            join trat in db.Tratamentos.AsNoTracking() on sess.TratamentoId equals trat.Id
            join doc in db.Documentos.AsNoTracking() on sess.DocumentoId equals doc.Id
            join ut in db.Utentes.AsNoTracking() on trat.UtenteId equals ut.Id
            join org in db.Organismos.AsNoTracking() on trat.OrganismoId equals org.Id into orgJ
            from org in orgJ.DefaultIfEmpty()
            where sess.DeletedOn == null && trat.DeletedOn == null && doc.DeletedOn == null
                  && doc.ClinicaId == clinicaId && doc.OrganismoId == organismoId
                  && doc.Data >= ini && doc.Data <= fim && sess.DocumentoId != null
                  && (sess.Faltou == 0 || org == null || org.Faltas == 0 || !org.ContabilizarFaltas)
            group sess by new
            {
                TratamentoId = trat.Id,
                trat.DataInic,
                trat.DataFim,
                trat.NumBenif,
                UtenteId = ut.Id,
                UtenteNome = ut.Nome,
                DocumentoId = doc.Id,
                doc.NumeroExibicao,
                doc.Data,
                doc.TotalLiquido,
            }
            into g
            select new RowBase(
                g.Key.TratamentoId, g.Key.DocumentoId, g.Key.DataInic, g.Key.DataFim,
                g.Select(x => x.NumSessao).Distinct().Count(),
                g.Key.UtenteId, g.Key.UtenteNome ?? "", g.Key.NumeroExibicao ?? "",
                g.Key.Data, g.Key.TotalLiquido ?? 0m, g.Max(x => x.NumDevolucao),
                (g.Key.NumBenif ?? "").Trim()))
            .ToListAsync(ct);

        if (filter.UtenteId.HasValue)
            rows = rows.Where(x => x.UtenteId == filter.UtenteId.Value).ToList();

        return await MapearComCopagamentosAsync(clinicaId, organismoId, rows, ct);
    }

    private async Task<List<AdseComunicacaoLinhaDTO>> ConsultasAsync(
        Guid clinicaId, Guid organismoId, AdseComunicacaoTableFilter filter, CancellationToken ct)
    {
        (DateTime ini, DateTime fim) = IntervaloFatura(filter);

        var rows = await (
            from c in db.Consultas.AsNoTracking()
            join a in db.Admissoes.AsNoTracking() on c.AdmissaoId equals a.Id
            join doc in db.Documentos.AsNoTracking() on c.DocumentoId equals doc.Id
            join ut in db.Utentes.AsNoTracking() on a.UtenteId equals ut.Id
            where c.DeletedOn == null && a.DeletedOn == null && doc.DeletedOn == null
                  && doc.ClinicaId == clinicaId && doc.OrganismoId == organismoId
                  && doc.Data >= ini && doc.Data <= fim && c.DocumentoId != null
            select new RowBase(
                a.Id, doc.Id, c.Data, c.Data, 1,
                ut.Id, ut.Nome ?? "", doc.NumeroExibicao ?? "", doc.Data,
                doc.TotalLiquido ?? 0m, null, string.Empty))
            .ToListAsync(ct);

        if (filter.UtenteId.HasValue)
            rows = rows.Where(x => x.UtenteId == filter.UtenteId.Value).ToList();

        return await MapearComCopagamentosAsync(clinicaId, organismoId, rows, ct);
    }

    private async Task<List<AdseComunicacaoLinhaDTO>> MapearComCopagamentosAsync(
        Guid clinicaId, Guid organismoId, List<RowBase> list, CancellationToken ct)
    {
        if (list.Count == 0)
            return [];

        HashSet<Guid> docIdSet = list.Select(x => x.DocumentoId).ToHashSet();

        // SQL Server 2014: evitar OPENJSON gerado por Contains(list) no EF.
        List<AdseCoPagamento> copsClinica = await db.AdseCoPagamentos.AsNoTracking()
            .Where(x => x.ClinicaId == clinicaId && x.DeletedOn == null)
            .ToListAsync(ct);
        Dictionary<Guid, AdseCoPagamento> cops = copsClinica
            .Where(x => docIdSet.Contains(x.DocumentoId))
            .ToDictionary(x => x.DocumentoId);

        List<AdsePreFatura> prefaturasClinica = await db.AdsePreFaturas.AsNoTracking()
            .Where(x => x.ClinicaId == clinicaId && x.DeletedOn == null)
            .ToListAsync(ct);
        Dictionary<(string Tipo, int NumOrdem), AdsePreFatura> prefaturas = prefaturasClinica
            .ToDictionary(x => (x.TipoPreFatura, x.NumOrdem));

        Dictionary<Guid, decimal> valorAdsePorTratamento = await (
            from st in db.ServicosTratamento.AsNoTracking()
            join t in db.Tratamentos.AsNoTracking() on st.TratamentoId equals t.Id
            where st.DeletedOn == null && t.DeletedOn == null && t.OrganismoId == organismoId
            group st by st.TratamentoId
            into g
            select new { TratamentoId = g.Key, Valor = g.Sum(x => x.ValorDesc ?? 0m) }
        ).ToDictionaryAsync(x => x.TratamentoId, x => x.Valor, ct);

        return list.Select(r =>
        {
            cops.TryGetValue(r.DocumentoId, out AdseCoPagamento? cop);
            int estado = cop?.Estado ?? AdseEstados.CoPagamentoNenhum;

            decimal valorAdse = cop?.ValorTotalAdse > 0
                ? cop.ValorTotalAdse
                : valorAdsePorTratamento.GetValueOrDefault(r.OrigemId, r.ValorFatura);

            AdsePreFatura? preFaturaRef = null;
            if (cop?.TipoPreFatura is not null && cop.NumOrdemPreFatura is int ordem)
                prefaturas.TryGetValue((cop.TipoPreFatura, ordem), out preFaturaRef);

            return new AdseComunicacaoLinhaDTO
            {
                Id = $"{r.OrigemId}:{r.DocumentoId}",
                OrigemClinicaId = r.OrigemId,
                DocumentoId = r.DocumentoId,
                CoPagamentoId = cop?.Id,
                NumeroOrigem = r.NumeroOrigem,
                DataInicio = r.DataInicio,
                DataFim = r.DataFim,
                NumeroSessoes = r.NumSessoes,
                UtenteId = r.UtenteId,
                UtenteNome = r.UtenteNome,
                NumeroFatura = r.NumeroFatura,
                DataFatura = r.DataFatura,
                ValorFatura = r.ValorFatura,
                ValorAdse = valorAdse,
                PreFatura = cop?.TipoPreFatura is not null && cop.NumOrdemPreFatura is int n
                    ? AdseEstados.CodigoPreFatura(cop.TipoPreFatura, n) : null,
                FaturaAdse = FormatarFaturaAdse(preFaturaRef),
                Estado = estado,
                EstadoDescricao = AdseEstados.DescricaoCoPagamento(estado),
                DataComunicacao = cop?.DataComunicacao,
                PdfFicheiro = cop?.PdfFicheiro,
                PdfRelatorioFicheiro = cop?.PdfRelatorioFicheiro,
                Erros = cop?.Erros,
                NumeroDevolucao = r.NumDevolucao,
            };
        }).OrderBy(x => x.DataFatura).ToList();
    }

    private static string? FormatarFaturaAdse(AdsePreFatura? pre)
    {
        if (pre?.ReferenciaNumeroDocumento is not int num)
            return null;
        if (!string.IsNullOrWhiteSpace(pre.ReferenciaSerie))
            return $"{pre.ReferenciaSerie.Trim()}/{num}";
        return num.ToString();
    }

    private static void AplicarFiltrosPosQuery(
        List<AdseComunicacaoLinhaDTO> linhas, AdseComunicacaoTableFilter filter)
    {
        if (filter.EstadoComunicacao is > 0)
            linhas.RemoveAll(x => x.Estado != filter.EstadoComunicacao);
        if (filter.Devolucoes)
            linhas.RemoveAll(x => string.IsNullOrWhiteSpace(x.NumeroDevolucao));
        else
            linhas.RemoveAll(x => !string.IsNullOrWhiteSpace(x.NumeroDevolucao));
    }

    private static (DateTime ini, DateTime fim) IntervaloFatura(AdseComunicacaoTableFilter filter)
    {
        DateTime ini = filter.DataInicial?.Date ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime fim = filter.DataFinal?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);
        return (ini, fim);
    }

    private sealed record RowBase(
        Guid OrigemId, Guid DocumentoId, DateTime? DataInicio, DateTime? DataFim, int NumSessoes,
        Guid UtenteId, string UtenteNome, string NumeroFatura, DateTime? DataFatura,
        decimal ValorFatura, string? NumDevolucao, string NumeroOrigem);
}
