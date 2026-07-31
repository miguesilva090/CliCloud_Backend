using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectEspHistoricoGateway(ApplicationDbContext dbContext) : ILoteDirectEspHistoricoGateway
{
    private sealed record EspHeaderRow(
        int Codigo,
        string? NumeroRequisicao,
        string? CodigoMedico,
        bool EnpAssinado
    );

    private sealed record EspMcdtRow(string CodMcdt);

    private sealed record EspEfetuadoRow(string CodigoMcdt, int NAmostras);

    private sealed record AcorInsRow(string Cservinst);

    public async Task UpdateParaHistoricoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        if (!await TabelaDisponivelAsync("dbo.RequisicoesEsp", cancellationToken).ConfigureAwait(false))
            return;

        _ = await dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE dbo.RequisicoesEsp
            SET Historico = 1
            WHERE NumeroRequisicao = {0} AND ISNULL(Apagado, 0) = 0
            """,
            [numeroRequisicao.Trim()],
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task UpdateParaAtivoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        if (!await TabelaDisponivelAsync("dbo.RequisicoesEsp", cancellationToken).ConfigureAwait(false))
            return;

        _ = await dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE dbo.RequisicoesEsp
            SET Historico = 0
            WHERE NumeroRequisicao = {0} AND ISNULL(Apagado, 0) = 0
            """,
            [numeroRequisicao.Trim()],
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task UpdateMedicoAsync(string numeroRequisicao, string codigoMedico, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao) || string.IsNullOrWhiteSpace(codigoMedico))
            return;

        if (!await TabelaDisponivelAsync("dbo.RequisicoesEsp", cancellationToken).ConfigureAwait(false))
            return;

        _ = await dbContext.Database.ExecuteSqlRawAsync(
            """
            UPDATE dbo.RequisicoesEsp
            SET CodigoMedico = {0}
            WHERE NumeroRequisicao = {1} AND ISNULL(Apagado, 0) = 0
            """,
            [codigoMedico.Trim(), numeroRequisicao.Trim()],
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task<LoteDirectEspRequisicaoData?> ObterPorCredencialAsync(
        string credencial,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credencial))
            return null;

        if (!await TabelaDisponivelAsync("dbo.RequisicoesEsp", cancellationToken).ConfigureAwait(false))
            return null;

        EspHeaderRow? header = await dbContext.Database
            .SqlQuery<EspHeaderRow>(
                $"""
                SELECT TOP 1
                    r.Codigo,
                    r.NumeroRequisicao,
                    r.CodigoMedico,
                    CAST(ISNULL(r.ENP_Assinado, 0) AS bit) AS EnpAssinado
                FROM dbo.RequisicoesEsp r
                WHERE r.NumeroRequisicao = {credencial.Trim()} AND ISNULL(r.Apagado, 0) = 0
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (header is null)
            return null;

        List<string> codigosMcdt = [];
        if (await TabelaDisponivelAsync("dbo.RequisicoesESPLinha", cancellationToken).ConfigureAwait(false))
        {
            List<EspMcdtRow> linhas = await dbContext.Database
                .SqlQuery<EspMcdtRow>(
                    $"""
                    SELECT LTRIM(RTRIM(rl.CodMCDT)) AS CodMcdt
                    FROM dbo.RequisicoesESPLinha rl
                    WHERE rl.CodigoRequisicaoESP = {header.Codigo}
                    """
                )
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            codigosMcdt = linhas
                .Select(x => x.CodMcdt?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        List<LoteDirectEspEfetuadoNaoPrescritoData> efetuados = [];
        if (await TabelaDisponivelAsync("dbo.EFETUADOS_N_PRESC", cancellationToken).ConfigureAwait(false))
        {
            List<EspEfetuadoRow> rows = await dbContext.Database
                .SqlQuery<EspEfetuadoRow>(
                    $"""
                    SELECT LTRIM(RTRIM(e.CodigoMCDT)) AS CodigoMcdt, ISNULL(e.NAmostras, 0) AS NAmostras
                    FROM dbo.EFETUADOS_N_PRESC e
                    WHERE e.CodigoRequisicaoESP = {header.Codigo}
                    """
                )
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            efetuados = rows
                .Where(x => !string.IsNullOrWhiteSpace(x.CodigoMcdt))
                .Select(x => new LoteDirectEspEfetuadoNaoPrescritoData
                {
                    CodigoMcdt = x.CodigoMcdt.Trim(),
                    NAmostras = x.NAmostras,
                })
                .ToList();
        }

        return new LoteDirectEspRequisicaoData
        {
            Codigo = header.Codigo,
            NumeroRequisicao = header.NumeroRequisicao,
            CodigoMedico = header.CodigoMedico,
            EnpAssinado = header.EnpAssinado,
            CodigosMcdt = codigosMcdt,
            EfetuadosNaoPrescritos = efetuados,
        };
    }

    public async Task<string?> ResolverCservinstAsync(
        int codigoOrganismo,
        string codigoServico,
        CancellationToken cancellationToken = default)
    {
        if (codigoOrganismo <= 0 || string.IsNullOrWhiteSpace(codigoServico))
            return null;

        if (!await TabelaDisponivelAsync("dbo.ACOR_INS", cancellationToken).ConfigureAwait(false))
            return null;

        AcorInsRow? row = await dbContext.Database
            .SqlQuery<AcorInsRow>(
                $"""
                SELECT TOP 1 LTRIM(RTRIM(a.cservinst)) AS Cservinst
                FROM dbo.ACOR_INS a
                WHERE a.c_instit = {codigoOrganismo}
                  AND a.c_servico = {codigoServico.Trim()}
                """
            )
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        string? cservinst = row?.Cservinst?.Trim();
        return string.IsNullOrWhiteSpace(cservinst) ? null : cservinst;
    }

    private async Task<bool> TabelaDisponivelAsync(string fullTableName, CancellationToken cancellationToken)
    {
        string sql = fullTableName switch
        {
            "dbo.RequisicoesEsp" => "SELECT OBJECT_ID(N'dbo.RequisicoesEsp', N'U') AS Value",
            "dbo.RequisicoesESPLinha" => "SELECT OBJECT_ID(N'dbo.RequisicoesESPLinha', N'U') AS Value",
            "dbo.EFETUADOS_N_PRESC" => "SELECT OBJECT_ID(N'dbo.EFETUADOS_N_PRESC', N'U') AS Value",
            "dbo.ACOR_INS" => "SELECT OBJECT_ID(N'dbo.ACOR_INS', N'U') AS Value",
            _ => throw new ArgumentOutOfRangeException(nameof(fullTableName), fullTableName, "Tabela ESP não suportada."),
        };

        int? objectId = await dbContext.Database
            .SqlQuery<int?>($"{sql}")
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return objectId is > 0;
    }
}
