using CliCloud.Application.Services.Credenciais.LoteDirectService;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;
using CliCloud.Domain.Entities.Credenciais;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Servicos;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence.Credenciais;

public sealed class LoteDirectCorrecaoLotesValidator(ApplicationDbContext dbContext) : ILoteDirectCorrecaoLotesValidator
{
    public async Task<ValidarCorrigirLotesDTO> ValidarAsync(int ano, int mes, CancellationToken cancellationToken = default)
    {
        List<LoteDirect> cabecalhos = await dbContext
            .Set<LoteDirect>()
            .AsNoTracking()
            .Where(x => !x.Historico && x.Ano == ano && x.Mes == mes)
            .ToListAsync(cancellationToken);

        // JOIN em vez de Contains(ids): SQL Server 2014 não suporta OPENJSON gerado pelo EF.
        IQueryable<LoteDirect> cabecalhosQuery = dbContext
            .Set<LoteDirect>()
            .AsNoTracking()
            .Where(x => !x.Historico && x.Ano == ano && x.Mes == mes);

        List<LoteDirectLinha> linhas = cabecalhos.Count == 0
            ? []
            : await dbContext
                .Set<LoteDirectLinha>()
                .AsNoTracking()
                .Join(
                    cabecalhosQuery,
                    linha => linha.LoteDirectId,
                    cabecalho => cabecalho.Id,
                    (linha, _) => linha)
                .ToListAsync(cancellationToken);

        List<LoteDirectLinha789> linhas789 = cabecalhos.Count == 0
            ? []
            : await dbContext
                .Set<LoteDirectLinha789>()
                .AsNoTracking()
                .Join(
                    cabecalhosQuery,
                    linha => linha.LoteDirectId,
                    cabecalho => cabecalho.Id,
                    (linha, _) => linha)
                .ToListAsync(cancellationToken);

        int agregadosExistentes = await dbContext
            .Set<LoteDirectAgregado>()
            .AsNoTracking()
            .CountAsync(x => x.Ano == ano && x.Mes == mes, cancellationToken);

        int semOrganismo = cabecalhos.Count(x => x.CodigoOrganismo is null);
        int semLinhasNemConsulta = cabecalhos.Count(cab =>
        {
            bool temLinhas = linhas.Any(x => x.LoteDirectId == cab.Id)
                || linhas789.Any(x => x.LoteDirectId == cab.Id);
            bool temConsulta = (cab.QuantidadeConsulta ?? 0) > 0 || (cab.ValorConsulta ?? 0) > 0;
            return !temLinhas && !temConsulta;
        });

        HashSet<(int CodigoOrganismo, Guid ServicoId)> precos = await CarregarPrecosAsync(
            cabecalhos,
            linhas,
            linhas789,
            cancellationToken
        );

        int linhasSemPreco = 0;
        foreach (LoteDirect cab in cabecalhos.Where(x => x.CodigoOrganismo.HasValue))
        {
            foreach (LoteDirectLinha linha in linhas.Where(x => x.LoteDirectId == cab.Id))
            {
                if (!precos.Contains((cab.CodigoOrganismo!.Value, linha.ServicoId)))
                    linhasSemPreco++;
            }

            foreach (LoteDirectLinha789 linha in linhas789.Where(x => x.LoteDirectId == cab.Id))
            {
                if (!precos.Contains((cab.CodigoOrganismo!.Value, linha.ServicoId)))
                    linhasSemPreco++;
            }
        }

        ValidarCorrigirLotesDTO result = new()
        {
            Ano = ano,
            Mes = mes,
            CabecalhosEncontrados = cabecalhos.Count,
            CabecalhosSemOrganismo = semOrganismo,
            CabecalhosSemLinhasNemConsulta = semLinhasNemConsulta,
            LinhasSemPreco = linhasSemPreco,
            AgregadosExistentes = agregadosExistentes,
        };

        if (cabecalhos.Count == 0)
        {
            result.Problemas.Add("Não existem lançamentos de credenciais para o mês/ano indicados.");
            result.PodeCorrigir = false;
            return result;
        }

        result.PodeCorrigir = true;

        if (semOrganismo > 0)
            result.Avisos.Add($"{semOrganismo} lançamento(s) sem organismo — os preços por subsistema não serão aplicados.");

        if (semLinhasNemConsulta > 0)
            result.Avisos.Add($"{semLinhasNemConsulta} lançamento(s) sem linhas de serviço nem consulta.");

        if (linhasSemPreco > 0)
            result.Avisos.Add($"{linhasSemPreco} linha(s) sem preço em SubsistemaServico para o organismo/serviço.");

        if (agregadosExistentes > 0)
            result.Avisos.Add($"{agregadosExistentes} lote(s) agregado(s) existente(s) serão substituídos.");

        return result;
    }

    private async Task<HashSet<(int CodigoOrganismo, Guid ServicoId)>> CarregarPrecosAsync(
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
            .Select(x => new { x.OrganismoId, x.ServicoId })
            .ToListAsync(cancellationToken);

        Dictionary<Guid, int> codigoPorOrganismoId = organismos.ToDictionary(x => x.Id, x => x.Codigo);

        return precosRaw
            .Where(x => organismoIds.Contains(x.OrganismoId) && servicoIds.Contains(x.ServicoId))
            .Where(x => codigoPorOrganismoId.ContainsKey(x.OrganismoId))
            .Select(x => (codigoPorOrganismoId[x.OrganismoId], x.ServicoId))
            .ToHashSet();
    }
}
