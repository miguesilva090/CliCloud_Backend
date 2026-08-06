using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectEspAccessor(ApplicationDbContext dbContext) : ILoteDirectEspHistoricoGateway
{
    public async Task UpdateParaHistoricoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        _ = await dbContext
            .Set<RequisicaoEsp>()
            .Where(x => x.NumeroRequisicao == numeroRequisicao.Trim())
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Historico, true), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateParaAtivoAsync(string numeroRequisicao, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao))
            return;

        _ = await dbContext
            .Set<RequisicaoEsp>()
            .Where(x => x.NumeroRequisicao == numeroRequisicao.Trim())
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Historico, false), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateMedicoAsync(string numeroRequisicao, string codigoMedico, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroRequisicao) || string.IsNullOrWhiteSpace(codigoMedico))
            return;

        string medico = codigoMedico.Trim();

        _ = await dbContext
            .Set<RequisicaoEsp>()
            .Where(x => x.NumeroRequisicao == numeroRequisicao.Trim())
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.CodigoMedico, medico), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<LoteDirectEspRequisicaoData?> ObterPorCredencialAsync(
        string credencial,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credencial))
            return null;

        RequisicaoEsp? header = await dbContext
            .Set<RequisicaoEsp>()
            .AsNoTracking()
            .Include(x => x.Linhas)
            .Include(x => x.EfetuadosNaoPrescritos)
            .FirstOrDefaultAsync(x => x.NumeroRequisicao == credencial.Trim(), cancellationToken)
            .ConfigureAwait(false);

        if (header is null)
            return null;

        List<string> codigosMcdt = header.Linhas
            .Select(x => x.CodigoMcdt.Trim())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        List<LoteDirectEspEfetuadoNaoPrescritoData> efetuados = header.EfetuadosNaoPrescritos
            .Where(x => !string.IsNullOrWhiteSpace(x.CodigoMcdt))
            .Select(x => new LoteDirectEspEfetuadoNaoPrescritoData
            {
                CodigoMcdt = x.CodigoMcdt.Trim(),
                NAmostras = x.NAmostras,
            })
            .ToList();

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

        string codigoServicoNormalizado = codigoServico.Trim();

        string? codigoMcdt = await (
            from ss in dbContext.Set<SubsistemaServico>()
            join s in dbContext.Set<Servico>() on ss.ServicoId equals s.Id
            join o in dbContext.Set<Organismo>() on ss.OrganismoId equals o.Id
            where o.CodigoULSNova == codigoOrganismo
                  && s.EAN == codigoServicoNormalizado
                  && !ss.Inativo
                  && ss.CodigoMcdt != null
                  && ss.CodigoMcdt != string.Empty
            select ss.CodigoMcdt
        )
        .FirstOrDefaultAsync(cancellationToken)
        .ConfigureAwait(false);

        codigoMcdt = codigoMcdt?.Trim();
        return string.IsNullOrWhiteSpace(codigoMcdt) ? null : codigoMcdt;
    }
}
