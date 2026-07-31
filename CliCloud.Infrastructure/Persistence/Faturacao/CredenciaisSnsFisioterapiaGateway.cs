using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService;
using CliCloud.Application.Services.Faturacao.CredenciaisSnsService.DTOs;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class CredenciaisSnsFisioterapiaGateway(ApplicationDbContext dbContext)
    : ICredenciaisSnsFisioterapiaGateway
{
    private const string BaseFromSql = """
        FROM dbo.LOTESPFISIO l
        INNER JOIN dbo.INSTITUI i ON l.codinst = i.c_instit
        INNER JOIN dbo.TIPOLOTES tl ON tl.codigo = l.codtipo
        INNER JOIN dbo.TIPO_SRV ts ON ts.c_tipo_srv = l.tiposerv AND ts.filtro = {0}
        WHERE 1 = 1
        """;

    private sealed record LotesPfisioListRow(
        int Indice,
        int NumeroLote,
        DateTime DataLote,
        decimal ValorTaxa,
        decimal Valor,
        int Ano,
        int Mes,
        int CodigoOrganismo,
        int TipoLote,
        int TipoServico,
        string? OrganismoNome,
        string? TipoLoteDesignacao,
        string? TipoServicoDesignacao
    );

    public async Task<PaginatedResponse<CredenciaisSnsLoteTableDTO>> GetPaginatedAsync(
        List<TableFilter> filters,
        int pageNumber,
        int pageSize,
        string? orderBy,
        int? filtroLegado,
        CancellationToken cancellationToken = default)
    {
        if (!await TabelaDisponivelAsync(cancellationToken).ConfigureAwait(false))
            return new PaginatedResponse<CredenciaisSnsLoteTableDTO>([], 0, pageNumber, pageSize);

        int filtro = filtroLegado ?? 0;
        FilterSqlResult filterSql = BuildFilterSql(filters);

        string countSql = $"""
            SELECT COUNT(*) AS Value
            {BaseFromSql}
            {filterSql.Clause}
            """;

        int total = await dbContext.Database
            .SqlQueryRaw<int>(countSql, [filtro, .. filterSql.Parameters])
            .FirstAsync(cancellationToken)
            .ConfigureAwait(false);

        if (total == 0)
            return new PaginatedResponse<CredenciaisSnsLoteTableDTO>([], 0, pageNumber, pageSize);

        string orderClause = ResolveOrderBy(orderBy);
        int skip = Math.Max(0, (pageNumber - 1) * pageSize);

        string dataSql = $"""
            SELECT
                l.indice AS Indice,
                ISNULL(l.c_lote, 0) AS NumeroLote,
                ISNULL(l.datalote, GETDATE()) AS DataLote,
                ISNULL(l.vtaxa, 0) AS ValorTaxa,
                ISNULL(l.valor, 0) AS Valor,
                ISNULL(l.ano, 0) AS Ano,
                ISNULL(l.nummes, 0) AS Mes,
                ISNULL(l.codinst, 0) AS CodigoOrganismo,
                ISNULL(l.codtipo, 0) AS TipoLote,
                ISNULL(l.tiposerv, 0) AS TipoServico,
                LTRIM(RTRIM(i.nome)) AS OrganismoNome,
                LTRIM(RTRIM(tl.designa)) AS TipoLoteDesignacao,
                LTRIM(RTRIM(ts.nome)) AS TipoServicoDesignacao
            {BaseFromSql}
            {filterSql.Clause}
            ORDER BY {orderClause}
            OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY
            """;

        List<LotesPfisioListRow> rows = await dbContext.Database
            .SqlQueryRaw<LotesPfisioListRow>(dataSql, [filtro, .. filterSql.Parameters])
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        List<CredenciaisSnsLoteTableDTO> data = rows.Select(MapRow).ToList();
        return new PaginatedResponse<CredenciaisSnsLoteTableDTO>(data, total, pageNumber, pageSize);
    }

    private static CredenciaisSnsLoteTableDTO MapRow(LotesPfisioListRow row)
    {
        string? mesNome = row.Mes is >= 1 and <= 12
            ? new[]
            {
                "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro",
            }[row.Mes - 1]
            : null;

        return new CredenciaisSnsLoteTableDTO
        {
            Id = IdFromIndice(row.Indice),
            Indice = row.Indice,
            NumeroLote = row.NumeroLote,
            DataLote = row.DataLote,
            ValorTaxa = row.ValorTaxa,
            Valor = row.Valor,
            Ano = row.Ano,
            Mes = row.Mes,
            MesNome = mesNome,
            CodigoOrganismo = row.CodigoOrganismo,
            OrganismoNome = row.OrganismoNome,
            TipoLote = row.TipoLote,
            TipoLoteDesignacao = row.TipoLoteDesignacao,
            TipoServico = row.TipoServico,
            TipoServicoDesignacao = row.TipoServicoDesignacao,
        };
    }

    private static Guid IdFromIndice(int indice)
        => Guid.Parse($"00000000-0000-4000-8000-{indice:D12}");

    private static string ResolveOrderBy(string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return "l.codinst ASC, l.c_lote ASC";

        return orderBy.ToLowerInvariant() switch
        {
            "numerolote asc" => "l.c_lote ASC",
            "numerolote desc" => "l.c_lote DESC",
            "datalote asc" => "l.datalote ASC",
            "datalote desc" => "l.datalote DESC",
            "codigoorganismo asc" => "l.codinst ASC",
            "codigoorganismo desc" => "l.codinst DESC",
            _ => "l.codinst ASC, l.c_lote ASC",
        };
    }

    private static FilterSqlResult BuildFilterSql(List<TableFilter> filters)
    {
        List<object> parameters = [];
        List<string> clauses = [];

        foreach (TableFilter filter in filters ?? [])
        {
            string id = (filter.Id ?? "").ToLowerInvariant();
            string? value = filter.Value?.Trim();
            if (string.IsNullOrWhiteSpace(value))
                continue;

            switch (id)
            {
                case "filtrobox":
                    if (int.TryParse(value, out int indiceOuLote))
                    {
                        parameters.Add(indiceOuLote);
                        clauses.Add($"(l.indice = {{{parameters.Count}}} OR l.c_lote = {{{parameters.Count}}})");
                    }
                    else
                    {
                        parameters.Add($"%{value}%");
                        clauses.Add($"CAST(l.c_lote AS varchar(20)) LIKE {{{parameters.Count}}}");
                    }
                    break;
                case "datalotede":
                    if (DateTime.TryParse(value, out DateTime de))
                    {
                        parameters.Add(de.Date);
                        clauses.Add($"l.datalote >= {{{parameters.Count}}}");
                    }
                    break;
                case "dataloteate":
                    if (DateTime.TryParse(value, out DateTime ate))
                    {
                        parameters.Add(ate.Date);
                        clauses.Add($"l.datalote <= {{{parameters.Count}}}");
                    }
                    break;
                case "numerolotede":
                    if (int.TryParse(value, out int loteDe))
                    {
                        parameters.Add(loteDe);
                        clauses.Add($"l.c_lote >= {{{parameters.Count}}}");
                    }
                    break;
                case "numeroloteate":
                    if (int.TryParse(value, out int loteAte))
                    {
                        parameters.Add(loteAte);
                        clauses.Add($"l.c_lote <= {{{parameters.Count}}}");
                    }
                    break;
                case "codigoorganismode":
                    if (int.TryParse(value, out int orgDe))
                    {
                        parameters.Add(orgDe);
                        clauses.Add($"l.codinst >= {{{parameters.Count}}}");
                    }
                    break;
                case "codigoorganismoate":
                    if (int.TryParse(value, out int orgAte))
                    {
                        parameters.Add(orgAte);
                        clauses.Add($"l.codinst <= {{{parameters.Count}}}");
                    }
                    break;
                case "anode":
                    if (int.TryParse(value, out int anoDe))
                    {
                        parameters.Add(anoDe);
                        clauses.Add($"l.ano >= {{{parameters.Count}}}");
                    }
                    break;
                case "anoate":
                    if (int.TryParse(value, out int anoAte))
                    {
                        parameters.Add(anoAte);
                        clauses.Add($"l.ano <= {{{parameters.Count}}}");
                    }
                    break;
                case "mesde":
                    if (int.TryParse(value, out int mesDe))
                    {
                        parameters.Add(mesDe);
                        clauses.Add($"l.nummes >= {{{parameters.Count}}}");
                    }
                    break;
                case "mesate":
                    if (int.TryParse(value, out int mesAte))
                    {
                        parameters.Add(mesAte);
                        clauses.Add($"l.nummes <= {{{parameters.Count}}}");
                    }
                    break;
                case "ano":
                    if (int.TryParse(value, out int ano))
                    {
                        parameters.Add(ano);
                        clauses.Add($"l.ano = {{{parameters.Count}}}");
                    }
                    break;
                case "mes":
                    if (int.TryParse(value, out int mes))
                    {
                        parameters.Add(mes);
                        clauses.Add($"l.nummes = {{{parameters.Count}}}");
                    }
                    break;
            }
        }

        string clause = clauses.Count == 0 ? string.Empty : " AND " + string.Join(" AND ", clauses);
        return new FilterSqlResult(clause, parameters);
    }

    private async Task<bool> TabelaDisponivelAsync(CancellationToken cancellationToken)
    {
        int? objectId = await dbContext.Database
            .SqlQuery<int?>(
                $"""
                SELECT OBJECT_ID(N'dbo.LOTESPFISIO', N'U') AS Value
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return objectId is > 0;
    }

    private sealed record FilterSqlResult(string Clause, List<object> Parameters);
}